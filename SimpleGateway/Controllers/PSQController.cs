using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleGateway.Data;
using SimpleGateway.Models;
using System.Security.Claims;
using QRCoder;

namespace SimpleGateway.Controllers
{
    public class PSQController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PSQController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper method for robust session validation
        private bool IsSessionValid(out string? username)
        {
            username = HttpContext.Session.GetString("username");
            
            Console.WriteLine($"=== PSQ SESSION VALIDATION ===");
            Console.WriteLine($"Session ID: {HttpContext.Session.Id}");
            Console.WriteLine($"Session IsAvailable: {HttpContext.Session.IsAvailable}");
            Console.WriteLine($"Username from session: '{username}'");
            
            // Check if session has all required data
            var displayName = HttpContext.Session.GetString("displayName");
            var role = HttpContext.Session.GetString("role");
            
            Console.WriteLine($"Session contents:");
            Console.WriteLine($"  - username: '{username}'");
            Console.WriteLine($"  - displayName: '{displayName}'");
            Console.WriteLine($"  - role: '{role}'");
            
            bool isValid = !string.IsNullOrEmpty(username);
            Console.WriteLine($"Session valid: {isValid}");
            Console.WriteLine($"=== END PSQ SESSION VALIDATION ===");
            
            return isValid;
        }

        // Generate unique 8-character alphanumeric code
        private string GenerateUniqueCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string code;
            
            do
            {
                code = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_context.PSQQuestionnaires.Any(q => q.UniqueCode == code));
            
            return code;
        }

        // Main PSQ Index page
        public async Task<IActionResult> Index(string? performerUsername = null)
        {
            Console.WriteLine($"=== PSQ INDEX START ===");
            Console.WriteLine($"Performer parameter: '{performerUsername}'");

            try
            {
                // Enhanced session validation
                if (!IsSessionValid(out string? currentUser))
                {
                    Console.WriteLine("PSQ: Session invalid, redirecting to login");
                    return RedirectToAction("Login", "Account");
                }

                Console.WriteLine($"PSQ: Current user from session: '{currentUser}'");

                // Determine target performer (default to current user if not specified)
                var targetPerformer = !string.IsNullOrEmpty(performerUsername) ? performerUsername : currentUser;
                Console.WriteLine($"PSQ: Target performer: '{targetPerformer}'");

                // Ensure PSQ tables exist using EF Core (safe method)
                try
                {
                    _context.Database.EnsureCreated();
                    Console.WriteLine("PSQ: Ensured PSQ tables exist using EF Core");
                }
                catch (Exception createEx)
                {
                    Console.WriteLine($"PSQ: Table creation issue (continuing): {createEx.Message}");
                }

                // Get user information
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == targetPerformer);
                if (user == null)
                {
                    Console.WriteLine($"PSQ: User not found: {targetPerformer}");
                    TempData["ErrorMessage"] = $"Performer '{targetPerformer}' not found.";
                    return RedirectToAction("Index", "Dashboard");
                }

                Console.WriteLine($"PSQ: Found user - ID: {user.Id}, Username: {user.Username}");

                // Check for existing active questionnaire
                var existingQuestionnaire = await _context.PSQQuestionnaires
                    .FirstOrDefaultAsync(q => q.PerformerId == user.Id && q.IsActive);

                Console.WriteLine($"PSQ: Existing questionnaire found: {existingQuestionnaire != null}");

                PSQResultsDto model;

                if (existingQuestionnaire != null)
                {
                    Console.WriteLine($"PSQ: Loading results for questionnaire {existingQuestionnaire.Id}");
                    model = await GetPSQResults(existingQuestionnaire.Id);
                    model.PerformerUsername = targetPerformer;
                    model.PerformerName = $"{user.FirstName} {user.LastName}";
                    model.HasActiveQuestionnaire = true;
                    model.UniqueCode = existingQuestionnaire.UniqueCode;
                    model.FeedbackUrl = Url.Action("Feedback", "PSQ", new { code = existingQuestionnaire.UniqueCode }, Request.Scheme);
                }
                else
                {
                    Console.WriteLine("PSQ: No existing questionnaire, creating empty model");
                    model = new PSQResultsDto
                    {
                        PerformerUsername = targetPerformer,
                        PerformerName = $"{user.FirstName} {user.LastName}",
                        TotalResponses = 0,
                        HasActiveQuestionnaire = false
                    };
                }

                // Set ViewBag properties for proper sidebar navigation
                ViewBag.ActiveSection = "PSQ";
                ViewBag.PerformerUsername = targetPerformer;
                ViewBag.PerformerName = model.PerformerName;

                Console.WriteLine($"PSQ: Returning model with {model.TotalResponses} responses");
                Console.WriteLine($"=== PSQ INDEX END ===");

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PSQ INDEX ERROR: {ex.Message}");
                Console.WriteLine($"PSQ INDEX STACK: {ex.StackTrace}");
                TempData["ErrorMessage"] = "An error occurred while loading the PSQ page. Please try again.";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        // Create new PSQ questionnaire
        [HttpPost]
        public async Task<IActionResult> CreateQuestionnaire(string performerUsername)
        {
            Console.WriteLine($"=== PSQ CREATE QUESTIONNAIRE START ===");
            Console.WriteLine($"Performer parameter: '{performerUsername}'");

            try
            {
                // Enhanced session validation
                if (!IsSessionValid(out string? currentUser))
                {
                    Console.WriteLine("PSQ: Session invalid during questionnaire creation");
                    return RedirectToAction("Login", "Account");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == performerUsername);
                if (user == null)
                {
                    TempData["ErrorMessage"] = $"Performer '{performerUsername}' not found.";
                    return RedirectToAction("Index");
                }

                // Check if active questionnaire already exists
                var existingQuestionnaire = await _context.PSQQuestionnaires
                    .FirstOrDefaultAsync(q => q.PerformerId == user.Id && q.IsActive);

                if (existingQuestionnaire != null)
                {
                    Console.WriteLine($"PSQ: Active questionnaire already exists: {existingQuestionnaire.UniqueCode}");
                    TempData["SuccessMessage"] = "PSQ questionnaire already exists and is active.";
                    return RedirectToAction("Index", new { performerUsername = performerUsername });
                }

                // Generate unique code
                var uniqueCode = GenerateUniqueCode();
                Console.WriteLine($"PSQ: Generated unique code: {uniqueCode}");

                // Create new questionnaire
                var questionnaire = new PSQQuestionnaire
                {
                    PerformerId = user.Id,
                    Title = $"PSQ Assessment for {user.Username}",
                    UniqueCode = uniqueCode,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PSQQuestionnaires.Add(questionnaire);
                await _context.SaveChangesAsync();

                Console.WriteLine($"PSQ: Created questionnaire with ID: {questionnaire.Id}");

                TempData["SuccessMessage"] = $"PSQ questionnaire created successfully! Share code: {uniqueCode}";
                return RedirectToAction("Index", new { performerUsername = performerUsername });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PSQ CREATE ERROR: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while creating the PSQ questionnaire. Please try again.";
                return RedirectToAction("Index", new { performerUsername = performerUsername });
            }
        }

        // Anonymous feedback form (GET)
        [HttpGet]
        public async Task<IActionResult> Feedback(string code)
        {
            Console.WriteLine($"=== PSQ FEEDBACK DEBUG ===");
            Console.WriteLine($"Code parameter received: '{code}'");

            if (string.IsNullOrEmpty(code))
            {
                Console.WriteLine("PSQ: No code provided");
                return NotFound("No code provided.");
            }

            try
            {
                var questionnaire = await _context.PSQQuestionnaires
                    .Include(q => q.Performer)
                    .FirstOrDefaultAsync(q => q.UniqueCode == code && q.IsActive);

                Console.WriteLine($"PSQ: Found questionnaire: {questionnaire != null}");

                if (questionnaire == null)
                {
                    Console.WriteLine("PSQ: Questionnaire not found or inactive");
                    return NotFound("Questionnaire not found or no longer active.");
                }

                var model = new SubmitPSQResponseDto
                {
                    QuestionnaireCode = code
                };

                ViewBag.PerformerName = $"{questionnaire.Performer?.FirstName} {questionnaire.Performer?.LastName}";
                ViewBag.QuestionnaireName = questionnaire.Title;

                Console.WriteLine($"PSQ: Returning feedback form for {ViewBag.PerformerName}");
                Console.WriteLine($"=== END PSQ FEEDBACK DEBUG ===");

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PSQ FEEDBACK ERROR: {ex.Message}");
                return NotFound("An error occurred while loading the feedback form.");
            }
        }

        // Submit feedback response (POST)
        [HttpPost]
        public async Task<IActionResult> Feedback(SubmitPSQResponseDto model)
        {
            Console.WriteLine($"=== PSQ FEEDBACK SUBMISSION START ===");
            Console.WriteLine($"Code: {model.QuestionnaireCode}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("PSQ: Model state invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"PSQ Validation Error: {error.ErrorMessage}");
                }
                return View(model);
            }

            try
            {
                var questionnaire = await _context.PSQQuestionnaires
                    .FirstOrDefaultAsync(q => q.UniqueCode == model.QuestionnaireCode && q.IsActive);

                if (questionnaire == null)
                {
                    Console.WriteLine("PSQ: Questionnaire not found during submission");
                    ModelState.AddModelError("", "Questionnaire not found or no longer active.");
                    return View(model);
                }

                // Create response with all 12 scores + 2 text fields
                var response = new PSQResponse
                {
                    PSQQuestionnaireId = questionnaire.Id,
                    SubmittedAt = DateTime.UtcNow,
                    
                    // 12 Patient Satisfaction Questions
                    PutMeAtEaseScore = model.PutMeAtEaseScore,
                    TreatedWithDignityScore = model.TreatedWithDignityScore,
                    ListenedToConcernsScore = model.ListenedToConcernsScore,
                    ExplainedTreatmentOptionsScore = model.ExplainedTreatmentOptionsScore,
                    InvolvedInDecisionsScore = model.InvolvedInDecisionsScore,
                    InvolvedFamilyScore = model.InvolvedFamilyScore,
                    TailoredApproachScore = model.TailoredApproachScore,
                    ExplainedNextStepsScore = model.ExplainedNextStepsScore,
                    ProvidedGuidanceScore = model.ProvidedGuidanceScore,
                    AllocatedTimeScore = model.AllocatedTimeScore,
                    WorkedWithTeamScore = model.WorkedWithTeamScore,
                    CanTrustDentistScore = model.CanTrustDentistScore,
                    
                    // 2 Text feedback fields
                    DoesWellComment = model.DoesWellComment?.Trim(),
                    CouldImproveComment = model.CouldImproveComment?.Trim()
                };

                _context.PSQResponses.Add(response);
                await _context.SaveChangesAsync();

                Console.WriteLine($"PSQ: Successfully saved response with ID: {response.Id}");
                Console.WriteLine($"=== PSQ FEEDBACK SUBMISSION END ===");

                return View("FeedbackSubmitted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PSQ SUBMISSION ERROR: {ex.Message}");
                Console.WriteLine($"PSQ SUBMISSION STACK: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while submitting your feedback. Please try again.");
                return View(model);
            }
        }

        // PSQ Results page
        public async Task<IActionResult> Results(string? performerUsername = null)
        {
            try
            {
                // Enhanced session validation
                if (!IsSessionValid(out string? currentUser))
                {
                    return RedirectToAction("Login", "Account");
                }

                var targetPerformer = !string.IsNullOrEmpty(performerUsername) ? performerUsername : currentUser;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == targetPerformer);
                if (user == null)
                {
                    TempData["ErrorMessage"] = $"Performer '{targetPerformer}' not found.";
                    return RedirectToAction("Index", "Dashboard");
                }

                var questionnaire = await _context.PSQQuestionnaires
                    .FirstOrDefaultAsync(q => q.PerformerId == user.Id && q.IsActive);

                PSQResultsDto model;

                if (questionnaire != null)
                {
                    model = await GetPSQResults(questionnaire.Id);
                    model.PerformerUsername = targetPerformer;
                    model.PerformerName = $"{user.FirstName} {user.LastName}";
                    model.HasActiveQuestionnaire = true;
                    model.UniqueCode = questionnaire.UniqueCode;
                    model.FeedbackUrl = Url.Action("Feedback", "PSQ", new { code = questionnaire.UniqueCode }, Request.Scheme);
                }
                else
                {
                    model = new PSQResultsDto
                    {
                        PerformerUsername = targetPerformer,
                        PerformerName = $"{user.FirstName} {user.LastName}",
                        TotalResponses = 0,
                        HasActiveQuestionnaire = false
                    };
                }

                // Set ViewBag properties for proper sidebar navigation
                ViewBag.ActiveSection = "PSQ";
                ViewBag.PerformerUsername = targetPerformer;
                ViewBag.PerformerName = model.PerformerName;

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PSQ RESULTS ERROR: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading PSQ results.";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        // Helper method to calculate PSQ results
        private async Task<PSQResultsDto> GetPSQResults(int questionnaireId)
        {
            var responses = await _context.PSQResponses
                .Where(r => r.PSQQuestionnaireId == questionnaireId)
                .ToListAsync();

            var results = new PSQResultsDto
            {
                TotalResponses = responses.Count
            };

            if (responses.Any())
            {
                // Calculate averages for the 12 rating questions (excluding "Not observed" values)
                var questions = new[]
                {
                    ("The Dentist put me at ease", responses.Where(r => r.PutMeAtEaseScore.HasValue && r.PutMeAtEaseScore.Value != 999).Select(r => (double)r.PutMeAtEaseScore.Value)),
                    ("Treated me with dignity and respect", responses.Where(r => r.TreatedWithDignityScore.HasValue && r.TreatedWithDignityScore.Value != 999).Select(r => (double)r.TreatedWithDignityScore.Value)),
                    ("Listened and responded to my concerns", responses.Where(r => r.ListenedToConcernsScore.HasValue && r.ListenedToConcernsScore.Value != 999).Select(r => (double)r.ListenedToConcernsScore.Value)),
                    ("Clearly explained treatment options", responses.Where(r => r.ExplainedTreatmentOptionsScore.HasValue && r.ExplainedTreatmentOptionsScore.Value != 999).Select(r => (double)r.ExplainedTreatmentOptionsScore.Value)),
                    ("Involved me in decisions about my care", responses.Where(r => r.InvolvedInDecisionsScore.HasValue && r.InvolvedInDecisionsScore.Value != 999).Select(r => (double)r.InvolvedInDecisionsScore.Value)),
                    ("Involved family/carers appropriately", responses.Where(r => r.InvolvedFamilyScore.HasValue && r.InvolvedFamilyScore.Value != 999).Select(r => (double)r.InvolvedFamilyScore.Value)),
                    ("Tailored approach to meet my needs", responses.Where(r => r.TailoredApproachScore.HasValue && r.TailoredApproachScore.Value != 999).Select(r => (double)r.TailoredApproachScore.Value)),
                    ("Explained what will happen next", responses.Where(r => r.ExplainedNextStepsScore.HasValue && r.ExplainedNextStepsScore.Value != 999).Select(r => (double)r.ExplainedNextStepsScore.Value)),
                    ("Provided guidance on dental care", responses.Where(r => r.ProvidedGuidanceScore.HasValue && r.ProvidedGuidanceScore.Value != 999).Select(r => (double)r.ProvidedGuidanceScore.Value)),
                    ("Allocated right amount of time", responses.Where(r => r.AllocatedTimeScore.HasValue && r.AllocatedTimeScore.Value != 999).Select(r => (double)r.AllocatedTimeScore.Value)),
                    ("Worked well with other team members", responses.Where(r => r.WorkedWithTeamScore.HasValue && r.WorkedWithTeamScore.Value != 999).Select(r => (double)r.WorkedWithTeamScore.Value)),
                    ("Can trust this dentist", responses.Where(r => r.CanTrustDentistScore.HasValue && r.CanTrustDentistScore.Value != 999).Select(r => (double)r.CanTrustDentistScore.Value))
                };

                results.QuestionAverages = questions
                    .Where(q => q.Item2.Any())
                    .ToDictionary(
                        q => q.Item1,
                        q => Math.Round(q.Item2.Average(), 2)
                    );

                // Collect text comments
                results.DoesWellComments = responses
                    .Where(r => !string.IsNullOrEmpty(r.DoesWellComment))
                    .Select(r => r.DoesWellComment!)
                    .ToList();

                results.CouldImproveComments = responses
                    .Where(r => !string.IsNullOrEmpty(r.CouldImproveComment))
                    .Select(r => r.CouldImproveComment!)
                    .ToList();
            }

            return results;
        }

        // Generate QR Code for feedback URL
        public IActionResult GenerateQRCode(string code)
        {
            try
            {
                var feedbackUrl = Url.Action("Feedback", "PSQ", new { code = code }, Request.Scheme);
                
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(feedbackUrl, QRCodeGenerator.ECCLevel.Q);
                    var qrCode = new BitmapByteQRCode(qrCodeData);
                    var qrCodeImage = qrCode.GetGraphic(20);
                    
                    return File(qrCodeImage, "image/png");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PSQ QR Code generation error: {ex.Message}");
                return NotFound("Error generating QR code");
            }
        }
    }
}
