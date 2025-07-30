using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleGateway.Data;
using SimpleGateway.Models;
using System.Security.Claims;
using QRCoder;

namespace SimpleGateway.Controllers
{
    public class MSFController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MSFController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Simple test action to verify controller is working
        public IActionResult Test()
        {
            Console.WriteLine("MSF: Test action called successfully");
            return Content("MSF Controller is working! Current time: " + DateTime.Now.ToString());
        }

        // Test feedback URL generation
        public IActionResult TestFeedback()
        {
            var testCode = "TESTCODE";
            var testUrl = Url.Action("Feedback", "MSF", new { code = testCode }, Request.Scheme);
            return Content($"Test feedback URL: {testUrl}");
        }

        // MSF Dashboard - Shows questionnaire status and results
        public async Task<IActionResult> Index(string performerUsername = null)
        {
            try
            {
                // Get current user from session
                var currentUser = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(currentUser))
                {
                    Console.WriteLine("MSF: No current user in session, redirecting to login");
                    return RedirectToAction("Login", "Account");
                }

                // If no performer specified, use current user
                var targetUsername = !string.IsNullOrEmpty(performerUsername) ? performerUsername : currentUser;
                Console.WriteLine($"MSF: Loading MSF for performer: {targetUsername}");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == targetUsername);
                if (user == null)
                {
                    Console.WriteLine($"MSF: User {targetUsername} not found, redirecting to login");
                    return RedirectToAction("Login", "Account");
                }

                Console.WriteLine($"MSF: Found user {user.Username} with ID {user.Id}");

                // Database connection verification - SAFE METHOD (following Database Integration Pattern)
                var canConnect = _context.Database.CanConnect();
                if (!canConnect)
                {
                    Console.WriteLine("MSF: Database connection issue");
                    TempData["ErrorMessage"] = "Database connection issue. Contact administrator.";
                    return RedirectToAction("Index", "Home");
                }

                Console.WriteLine("MSF: Database connection verified");

                // Check if MSF tables exist before querying
                MSFQuestionnaire questionnaire = null;
                try
                {
                    // Try to query for existing questionnaire - this will fail if tables don't exist
                    questionnaire = await _context.MSFQuestionnaires
                        .Include(q => q.Responses)
                        .FirstOrDefaultAsync(q => q.PerformerId == user.Id && q.IsActive);
                    
                    Console.WriteLine($"MSF: Found existing questionnaire: {questionnaire != null}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"MSF: Tables don't exist yet or query error: {ex.Message}");
                    questionnaire = null;
                }

                // If no questionnaire exists, show creation form
                if (questionnaire == null)
                {
                    Console.WriteLine("MSF: No questionnaire exists, showing creation form");
                    ViewBag.ShowCreateForm = true;
                    ViewBag.PerformerUsername = targetUsername;
                    ViewBag.PerformerName = user.Username;
                    return View((MSFQuestionnaire)null);
                }

                // Generate QR code for the feedback URL (skip QR generation for now to avoid errors)
                var feedbackUrl = Url.Action("Feedback", "MSF", new { code = questionnaire.UniqueCode }, Request.Scheme);
                var qrCodeImage = ""; // Skip QR code generation for debugging

                Console.WriteLine($"MSF: Generated feedback URL for display: {feedbackUrl}");
                Console.WriteLine($"MSF: Questionnaire unique code: {questionnaire.UniqueCode}");

                ViewBag.FeedbackUrl = feedbackUrl;
                ViewBag.QRCodeImage = qrCodeImage;
                ViewBag.ResponseCount = questionnaire.Responses?.Count ?? 0;
                ViewBag.ShowCreateForm = false;

                Console.WriteLine($"MSF: Successfully loaded MSF dashboard for {user.Username}");
                return View(questionnaire);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSF: Unexpected error in Index: {ex.Message}");
                Console.WriteLine($"MSF: Stack trace: {ex.StackTrace}");
                TempData["ErrorMessage"] = $"Unexpected error loading MSF: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Create new MSF questionnaire
        [HttpPost]
        public async Task<IActionResult> CreateQuestionnaire(string performerUsername)
        {
            try
            {
                var currentUser = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(currentUser))
                    return RedirectToAction("Login", "Account");

                var targetUsername = !string.IsNullOrEmpty(performerUsername) ? performerUsername : currentUser;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == targetUsername);
                if (user == null)
                    return RedirectToAction("Login", "Account");

                // Create new questionnaire
                var uniqueCode = GenerateUniqueCode();
                var questionnaire = new MSFQuestionnaire
                {
                    PerformerId = user.Id,
                    Title = $"MSF Assessment for {user.Username}",
                    UniqueCode = uniqueCode,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                Console.WriteLine($"MSF: Creating new questionnaire for {user.Username}");
                Console.WriteLine($"MSF: Generated unique code: {uniqueCode}");
                _context.MSFQuestionnaires.Add(questionnaire);
                await _context.SaveChangesAsync();
                Console.WriteLine($"MSF: Successfully created questionnaire with ID {questionnaire.Id} and code {questionnaire.UniqueCode}");

                // Generate feedback URL for verification
                var testFeedbackUrl = Url.Action("Feedback", "MSF", new { code = questionnaire.UniqueCode }, Request.Scheme);
                Console.WriteLine($"MSF: Generated feedback URL: {testFeedbackUrl}");

                TempData["SuccessMessage"] = $"MSF assessment link created successfully! Code: {questionnaire.UniqueCode}";
                return RedirectToAction("Index", new { performerUsername = targetUsername });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSF: Error creating questionnaire: {ex.Message}");
                TempData["ErrorMessage"] = "Error creating MSF assessment. Please try again.";
                return RedirectToAction("Index", new { performerUsername });
            }
        }

        // Anonymous feedback form
        [HttpGet]
        public async Task<IActionResult> Feedback(string code)
        {
            if (string.IsNullOrEmpty(code))
                return NotFound();

            var questionnaire = await _context.MSFQuestionnaires
                .Include(q => q.Performer)
                .FirstOrDefaultAsync(q => q.UniqueCode == code && q.IsActive);

            if (questionnaire == null)
                return NotFound("Questionnaire not found or no longer active.");

            ViewBag.PerformerName = questionnaire.Performer.Username;
            ViewBag.QuestionnaireTitle = questionnaire.Title;

            return View(new SubmitMSFResponseDto { QuestionnaireCode = code });
        }

        // Submit anonymous feedback
        [HttpPost]
        public async Task<IActionResult> Feedback(SubmitMSFResponseDto model)
        {
            if (!ModelState.IsValid)
            {
                var questionnaire = await _context.MSFQuestionnaires
                    .Include(q => q.Performer)
                    .FirstOrDefaultAsync(q => q.UniqueCode == model.QuestionnaireCode && q.IsActive);

                if (questionnaire != null)
                {
                    ViewBag.PerformerName = questionnaire.Performer.Username;
                    ViewBag.QuestionnaireTitle = questionnaire.Title;
                }

                return View(model);
            }

            var targetQuestionnaire = await _context.MSFQuestionnaires
                .FirstOrDefaultAsync(q => q.UniqueCode == model.QuestionnaireCode && q.IsActive);

            if (targetQuestionnaire == null)
                return NotFound("Questionnaire not found or no longer active.");

            // Create MSF response
            var response = new MSFResponse
            {
                MSFQuestionnaireId = targetQuestionnaire.Id,
                RespondentName = model.RespondentName,
                RespondentRole = model.RespondentRole,
                SubmittedAt = DateTime.UtcNow,

                // Patient Care & Communication (1-6)
                PatientCareQualityScore = model.PatientCareQualityScore,
                PatientCareQualityComment = model.PatientCareQualityComment,
                CommunicationSkillsScore = model.CommunicationSkillsScore,
                CommunicationSkillsComment = model.CommunicationSkillsComment,
                CommunicationEmpathyScore = model.CommunicationEmpathyScore,
                CommunicationEmpathyComment = model.CommunicationEmpathyComment,
                HistoryTakingScore = model.HistoryTakingScore,
                HistoryTakingComment = model.HistoryTakingComment,
                ConsultationManagementScore = model.ConsultationManagementScore,
                ConsultationManagementComment = model.ConsultationManagementComment,
                CulturalSensitivityScore = model.CulturalSensitivityScore,
                CulturalSensitivityComment = model.CulturalSensitivityComment,

                // Professional Integrity & Development (7-11)
                EthicalProfessionalismScore = model.EthicalProfessionalismScore,
                EthicalProfessionalismComment = model.EthicalProfessionalismComment,
                ProfessionalDevelopmentScore = model.ProfessionalDevelopmentScore,
                ProfessionalDevelopmentComment = model.ProfessionalDevelopmentComment,
                TechnicalCompetenceScore = model.TechnicalCompetenceScore,
                TechnicalCompetenceComment = model.TechnicalCompetenceComment,
                DecisionMakingScore = model.DecisionMakingScore,
                DecisionMakingComment = model.DecisionMakingComment,
                DocumentationScore = model.DocumentationScore,
                DocumentationComment = model.DocumentationComment,

                // Team Working & Quality Improvement (12-17)
                TeamCollaborationScore = model.TeamCollaborationScore,
                TeamCollaborationComment = model.TeamCollaborationComment,
                TeamSupportScore = model.TeamSupportScore,
                TeamSupportComment = model.TeamSupportComment,
                LeadershipSkillsScore = model.LeadershipSkillsScore,
                LeadershipSkillsComment = model.LeadershipSkillsComment,
                QualityImprovementScore = model.QualityImprovementScore,
                QualityImprovementComment = model.QualityImprovementComment,
                HealthSafetyAwarenessScore = model.HealthSafetyAwarenessScore,
                HealthSafetyAwarenessComment = model.HealthSafetyAwarenessComment,
                ContinuousImprovementScore = model.ContinuousImprovementScore,
                ContinuousImprovementComment = model.ContinuousImprovementComment,

                AdditionalComments = model.AdditionalComments
            };

            try
            {
                // Database connection verification - SAFE METHOD (following Database Integration Pattern)
                var canConnect = _context.Database.CanConnect();
                if (!canConnect)
                {
                    Console.WriteLine("MSF: Database connection issue during feedback submission");
                    TempData["ErrorMessage"] = "Database connection issue. Please try again.";
                    return View(model);
                }

                Console.WriteLine($"MSF: Submitting feedback for questionnaire {targetQuestionnaire.Id} from {model.RespondentName}");
                _context.MSFResponses.Add(response);
                await _context.SaveChangesAsync();
                Console.WriteLine($"MSF: Successfully saved feedback response");

                return View("FeedbackSubmitted");
            }
            catch (Exception ex)
            {
                // ✅ SAFE ERROR HANDLING - Log errors without destroying data
                Console.WriteLine($"MSF: Database error saving feedback: {ex.Message}");
                TempData["ErrorMessage"] = "Error saving feedback. Please try again.";
                
                // Reload questionnaire info for the view
                var questionnaire = await _context.MSFQuestionnaires
                    .Include(q => q.Performer)
                    .FirstOrDefaultAsync(q => q.UniqueCode == model.QuestionnaireCode && q.IsActive);

                if (questionnaire != null)
                {
                    ViewBag.PerformerName = questionnaire.Performer.Username;
                    ViewBag.QuestionnaireTitle = questionnaire.Title;
                }

                return View(model);
            }
        }

        // View MSF Results
        public async Task<IActionResult> Results()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var questionnaire = await _context.MSFQuestionnaires
                .Include(q => q.Responses)
                .FirstOrDefaultAsync(q => q.PerformerId == user.Id && q.IsActive);

            if (questionnaire == null || !questionnaire.Responses.Any())
            {
                ViewBag.Message = "No feedback responses available yet.";
                return View(new MSFResultsDto());
            }

            // Calculate aggregated results
            var results = CalculateResults(questionnaire.Responses.ToList());
            return View(results);
        }

        // Deactivate current questionnaire and create a new one
        [HttpPost]
        public async Task<IActionResult> StartNewAssessment()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Deactivate current questionnaire
            var currentQuestionnaire = await _context.MSFQuestionnaires
                .FirstOrDefaultAsync(q => q.PerformerId == user.Id && q.IsActive);

            if (currentQuestionnaire != null)
            {
                currentQuestionnaire.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private string GenerateUniqueCode()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpper();
        }

        private string GenerateQRCode(string url)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qrCode.GetGraphic(20);
                return Convert.ToBase64String(qrCodeBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        private MSFResultsDto CalculateResults(List<MSFResponse> responses)
        {
            var results = new MSFResultsDto
            {
                TotalResponses = responses.Count,
                ResponseSummary = responses.Select(r => new ResponseSummary
                {
                    RespondentRole = r.RespondentRole ?? "Not specified",
                    SubmittedAt = r.SubmittedAt
                }).ToList()
            };

            // Calculate averages for each question (only include responses that have scores)
            var questions = new[]
            {
                ("Patient Care Quality", responses.Where(r => r.PatientCareQualityScore.HasValue).Select(r => r.PatientCareQualityScore!.Value)),
                ("Communication Skills", responses.Where(r => r.CommunicationSkillsScore.HasValue).Select(r => r.CommunicationSkillsScore!.Value)),
                ("Communication & Empathy", responses.Where(r => r.CommunicationEmpathyScore.HasValue).Select(r => r.CommunicationEmpathyScore!.Value)),
                ("History Taking", responses.Where(r => r.HistoryTakingScore.HasValue).Select(r => r.HistoryTakingScore!.Value)),
                ("Consultation Management", responses.Where(r => r.ConsultationManagementScore.HasValue).Select(r => r.ConsultationManagementScore!.Value)),
                ("Cultural Sensitivity", responses.Where(r => r.CulturalSensitivityScore.HasValue).Select(r => r.CulturalSensitivityScore!.Value)),
                ("Ethical Professionalism", responses.Where(r => r.EthicalProfessionalismScore.HasValue).Select(r => r.EthicalProfessionalismScore!.Value)),
                ("Professional Development", responses.Where(r => r.ProfessionalDevelopmentScore.HasValue).Select(r => r.ProfessionalDevelopmentScore!.Value)),
                ("Technical Competence", responses.Where(r => r.TechnicalCompetenceScore.HasValue).Select(r => r.TechnicalCompetenceScore!.Value)),
                ("Decision Making", responses.Where(r => r.DecisionMakingScore.HasValue).Select(r => r.DecisionMakingScore!.Value)),
                ("Documentation", responses.Where(r => r.DocumentationScore.HasValue).Select(r => r.DocumentationScore!.Value)),
                ("Team Collaboration", responses.Where(r => r.TeamCollaborationScore.HasValue).Select(r => r.TeamCollaborationScore!.Value)),
                ("Team Support", responses.Where(r => r.TeamSupportScore.HasValue).Select(r => r.TeamSupportScore!.Value)),
                ("Leadership Skills", responses.Where(r => r.LeadershipSkillsScore.HasValue).Select(r => r.LeadershipSkillsScore!.Value)),
                ("Quality Improvement", responses.Where(r => r.QualityImprovementScore.HasValue).Select(r => r.QualityImprovementScore!.Value)),
                ("Health & Safety Awareness", responses.Where(r => r.HealthSafetyAwarenessScore.HasValue).Select(r => r.HealthSafetyAwarenessScore!.Value)),
                ("Continuous Improvement", responses.Where(r => r.ContinuousImprovementScore.HasValue).Select(r => r.ContinuousImprovementScore!.Value))
            };

            results.QuestionAverages = questions
                .Where(q => q.Item2.Any())
                .ToDictionary(
                    q => q.Item1,
                    q => Math.Round(q.Item2.Average(), 2)
                );

            // Collect all comments
            results.AllComments = responses
                .SelectMany(r => new[]
                {
                    r.PatientCareQualityComment,
                    r.CommunicationSkillsComment,
                    r.CommunicationEmpathyComment,
                    r.HistoryTakingComment,
                    r.ConsultationManagementComment,
                    r.CulturalSensitivityComment,
                    r.EthicalProfessionalismComment,
                    r.ProfessionalDevelopmentComment,
                    r.TechnicalCompetenceComment,
                    r.DecisionMakingComment,
                    r.DocumentationComment,
                    r.TeamCollaborationComment,
                    r.TeamSupportComment,
                    r.LeadershipSkillsComment,
                    r.QualityImprovementComment,
                    r.HealthSafetyAwarenessComment,
                    r.ContinuousImprovementComment,
                    r.AdditionalComments
                })
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c!) // Convert nullable to non-nullable
                .ToList();

            return results;
        }
    }
}
