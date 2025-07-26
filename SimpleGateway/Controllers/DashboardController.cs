using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;
using SimpleGateway.Data;

namespace SimpleGateway.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentRole = currentRole;

            if (currentRole == "performer")
            {
                // Set proper ViewBag properties for performer's own dashboard
                ViewBag.PerformerUsername = currentUser;
                ViewBag.IsOwnDashboard = true;
                ViewBag.CanEdit = true;
                
                // Get performer's name for display
                var performer = _context.Users.FirstOrDefault(u => u.Username == currentUser);
                if (performer != null)
                {
                    ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
                }
                
                // Show performer dashboard instead of redirecting to details
                return View("PerformerDashboard");
            }
            else if (currentRole == "admin" || currentRole == "superuser")
            {
                // Redirect admin/superuser to dedicated admin dashboard
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                // Show list of performers for advisors and supervisors
                List<UserModel> assignedPerformers = new List<UserModel>();
                
                if (currentRole == "supervisor")
                {
                    // Supervisors can only see performers assigned to them
                    var currentUserId = _context.Users.FirstOrDefault(u => u.Username == currentUser)?.Id;
                    Console.WriteLine($"DEBUG: Supervisor {currentUser} has ID: {currentUserId}");
                    
                    if (currentUserId.HasValue)
                    {
                        assignedPerformers = _context.Assignments
                            .Include(a => a.Performer)
                            .Where(a => a.SupervisorId == currentUserId.Value && a.IsActive)
                            .Select(a => a.Performer)
                            .Where(p => p != null)
                            .ToList()!;
                        
                        Console.WriteLine($"DEBUG: Found {assignedPerformers.Count} assigned performers for supervisor {currentUser}");
                    }
                }
                else if (currentRole == "advisor")
                {
                    // Advisors can only see performers assigned to them
                    var currentUserId = _context.Users.FirstOrDefault(u => u.Username == currentUser)?.Id;
                    if (currentUserId.HasValue)
                    {
                        assignedPerformers = _context.Assignments
                            .Include(a => a.Performer)
                            .Where(a => a.AdvisorId == currentUserId.Value && a.IsActive)
                            .Select(a => a.Performer)
                            .Where(p => p != null)
                            .ToList()!;
                    }
                }
                
                ViewBag.Users = assignedPerformers;
                return View();
            }
        }

        public IActionResult PerformerDetails(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = "PerformerDetails";

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Get or create performer details from database
            Console.WriteLine($"DASHBOARD DEBUG: Attempting to retrieve performer details for {performerUsername}");
            var model = _context.PerformerDetails.FirstOrDefault(p => p.Username == performerUsername);
            if (model == null)
            {
                Console.WriteLine($"DASHBOARD DEBUG: No existing details found for {performerUsername}, creating new");
                // Create new performer details if not exists
                model = new PerformerDetailsModel
                {
                    Username = performerUsername
                };
            }
            else
            {
                Console.WriteLine($"DASHBOARD DEBUG: Found existing details for {performerUsername} - FirstName: {model.FirstName}, LastName: {model.LastName}, GDC: {model.GDCNumber}");
            }

            // Check total PerformerDetails count
            var totalPerformerDetails = _context.PerformerDetails.Count();
            Console.WriteLine($"DASHBOARD DEBUG: Total PerformerDetails records in database: {totalPerformerDetails}");

            return View("Performer/PerformerDetails", model);
        }

        [HttpPost]
        public IActionResult PerformerDetails(PerformerDetailsModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            Console.WriteLine($"DASHBOARD DEBUG: POST request received for PerformerDetails, user: {currentUser}, model.Username: {model?.Username}");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine($"DASHBOARD DEBUG: No current user in session, redirecting to login");
                return RedirectToAction("Login", "Account");
            }

            if (model == null)
            {
                Console.WriteLine($"DASHBOARD DEBUG: Model is null");
                return RedirectToAction("PerformerDetails", new { performerUsername = currentUser });
            }

            Console.WriteLine($"DASHBOARD DEBUG: ModelState.IsValid = {ModelState.IsValid}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"DASHBOARD DEBUG: ModelState errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"DASHBOARD DEBUG: - {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if performer details already exists
                    var existingDetails = _context.PerformerDetails.FirstOrDefault(p => p.Username == model.Username);
                    
                    if (existingDetails != null)
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Updating existing performer details for {model.Username}");
                        Console.WriteLine($"DASHBOARD DEBUG: Before update - FirstName: '{existingDetails.FirstName}' -> '{model.FirstName}'");
                        // Update existing record
                        existingDetails.FirstName = model.FirstName;
                        existingDetails.LastName = model.LastName;
                        existingDetails.GDCNumber = model.GDCNumber;
                        existingDetails.Email = model.Email;
                        existingDetails.ContactNumber = model.ContactNumber;
                        existingDetails.SupportingDentist = model.SupportingDentist;
                        existingDetails.SupportingDentistContactNumber = model.SupportingDentistContactNumber;
                        existingDetails.PracticeAddress = model.PracticeAddress;
                        existingDetails.PracticePostCode = model.PracticePostCode;
                        existingDetails.UniversityCountryOfQualification = model.UniversityCountryOfQualification;
                        existingDetails.DateOfDentalQualification = model.DateOfDentalQualification;
                        existingDetails.DateOfUKRegistration = model.DateOfUKRegistration;
                        existingDetails.IsCompleted = model.IsCompleted;
                        
                        _context.PerformerDetails.Update(existingDetails);
                        Console.WriteLine($"DASHBOARD DEBUG: After update - FirstName: '{existingDetails.FirstName}'");
                    }
                    else
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Creating new performer details for {model.Username}");
                        Console.WriteLine($"DASHBOARD DEBUG: New record - FirstName: '{model.FirstName}', LastName: '{model.LastName}'");
                        // Add new record
                        _context.PerformerDetails.Add(model);
                    }
                    
                    var savedRows = _context.SaveChanges();
                    Console.WriteLine($"DASHBOARD DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                    
                    if (savedRows > 0)
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Save successful! Redirecting with success message");
                        TempData["SuccessMessage"] = "Details saved successfully!";
                        return RedirectToAction("PerformerDetails", new { performerUsername = model.Username });
                    }
                    else
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Save failed - no rows affected");
                        TempData["ErrorMessage"] = "Save failed - no changes were made to the database.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Exception during save: {ex.Message}");
                    Console.WriteLine($"DASHBOARD DEBUG: Stack trace: {ex.StackTrace}");
                    TempData["ErrorMessage"] = $"Error saving details: {ex.Message}";
                }
            }
            else
            {
                Console.WriteLine($"DASHBOARD DEBUG: ModelState invalid, redisplaying form");
                TempData["ErrorMessage"] = "Please correct the validation errors and try again.";
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PerformerUsername = model.Username;
            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentUserRole = HttpContext.Session.GetString("role");
            ViewBag.IsOwnDashboard = (currentUser == model.Username);
            ViewBag.ActiveSection = "PerformerDetails";
            
            var performer2 = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (performer2 != null)
            {
                ViewBag.PerformerName = $"{performer2.FirstName} {performer2.LastName}";
            }
            
            return View("Performer/PerformerDetails", model);
        }

        public IActionResult Uploads(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "Uploads");
        }

        public IActionResult PreviousExperience(string performerUsername)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = "PreviousExperience";
            ViewBag.IsAdvisor = (currentRole == "advisor");

            // Get performer's name for display
            var performer = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer != null)
            {
                ViewBag.PerformerName = $"{performer.FirstName} {performer.LastName}";
            }

            // Get or create simple previous experience from existing table
            Console.WriteLine($"DASHBOARD DEBUG: Attempting to retrieve previous experience for {performerUsername}");
            
            // Use existing PreviousExperiences table but create a simple model for the form
            var existingData = _context.PreviousExperiences.FirstOrDefault(p => p.Username == performerUsername);
            
            SimplePreviousExperienceModel model;
            if (existingData == null)
            {
                Console.WriteLine($"DASHBOARD DEBUG: No existing previous experience found for {performerUsername}, creating new");
                model = new SimplePreviousExperienceModel
                {
                    Username = performerUsername
                };
            }
            else
            {
                Console.WriteLine($"DASHBOARD DEBUG: Found existing previous experience for {performerUsername}");
                // Map existing complex data to simple model
                model = new SimplePreviousExperienceModel
                {
                    Username = performerUsername,
                    PreviousEmployer = existingData.NhsExperience ?? "",
                    JobTitle = "",
                    StartDate = "",
                    EndDate = "",
                    JobDescription = existingData.GdcGapsExplanation ?? "",
                    QualificationsSummary = "",
                    ClinicalExperience = "",
                    SkillsAndCompetencies = "",
                    AdditionalNotes = ""
                };
            }

            return View("Performer/SimplePreviousExperience", model);
        }

        [HttpGet]
        public IActionResult PreviousExperienceForm(string performerUsername)
        {
            // Redirect GET requests to the PreviousExperience action
            return RedirectToAction("PreviousExperience", new { performerUsername = performerUsername });
        }

        [HttpPost]
        public IActionResult PreviousExperienceForm(PreviousExperienceModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Debug: Log what we received
            Console.WriteLine($"DASHBOARD DEBUG: POST received - Username: {model?.Username}");
            Console.WriteLine($"DASHBOARD DEBUG: ModelState.IsValid: {ModelState.IsValid}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"DASHBOARD DEBUG: ModelState errors:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"DASHBOARD DEBUG: {error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                // Clear model state errors - we want to allow saving even with validation errors
                ModelState.Clear();
                Console.WriteLine($"DASHBOARD DEBUG: Cleared ModelState errors - proceeding with save");
            }

            // Always allow saving, even with empty fields - skip model validation
            Console.WriteLine($"DASHBOARD DEBUG: Attempting to save PreviousExperience for {model?.Username ?? "null"}");

            try
            {
                // Instead of migrating during save, check if the table has the new columns
                try
                {
                    // Test if the new columns exist by trying a simple query
                    var testQuery = _context.PreviousExperiences.Select(p => new { p.Id, p.QualificationsJson }).FirstOrDefault();
                    Console.WriteLine($"DASHBOARD DEBUG: New JSON columns are available in database");
                }
                catch (Exception columnEx)
                {
                    Console.WriteLine($"DASHBOARD DEBUG: JSON columns not available yet: {columnEx.Message}");
                    TempData["ErrorMessage"] = "Database is still updating. Please try again in a few moments.";
                    return RedirectToAction("PreviousExperience", new { performerUsername = model?.Username ?? currentUser });
                }

                // Ensure model has username
                if (string.IsNullOrEmpty(model?.Username))
                {
                    if (model != null)
                        model.Username = currentUser;
                    else
                    {
                        Console.WriteLine($"DASHBOARD DEBUG: Model is null, creating new model");
                        model = new PreviousExperienceModel { Username = currentUser };
                    }
                }

                // Check if record already exists
                var existingRecord = _context.PreviousExperiences.FirstOrDefault(p => p.Username == model.Username);
                
                if (existingRecord != null)
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Updating existing PreviousExperience record for {model.Username}");
                    // Update existing record - handle nullable strings properly
                    existingRecord.GdcGapsExplanation = model.GdcGapsExplanation;
                    existingRecord.NhsExperience = model.NhsExperience;
                    existingRecord.FullTime = model.FullTime;
                    existingRecord.PartTimeDaysPerWeek = model.PartTimeDaysPerWeek;
                    existingRecord.Years = model.Years;
                    existingRecord.Months = model.Months;
                    existingRecord.AdvisorDeclarationBy = model.AdvisorDeclarationBy;
                    existingRecord.AdvisorDeclarationComment = model.AdvisorDeclarationComment;
                    existingRecord.ApplicantConfirmed = model.ApplicantConfirmed;
                    existingRecord.ApplicantConfirmedAt = model.ApplicantConfirmed ? DateTime.UtcNow : null;
                    existingRecord.IsSubmitted = model.IsSubmitted;
                    existingRecord.UpdatedAt = DateTime.UtcNow;
                    
                    // Update JSON data using the helper properties
                    existingRecord.Qualifications = model.Qualifications ?? new List<Qualification>();
                    existingRecord.EmploymentHistory = model.EmploymentHistory ?? new List<EmploymentHistoryJob>();
                    existingRecord.ClinicalExperience = model.ClinicalExperience ?? new List<ClinicalProcedureEntry>();
                    
                    _context.PreviousExperiences.Update(existingRecord);
                    Console.WriteLine($"DASHBOARD DEBUG: Updated existing record for {model.Username}");
                }
                else
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Creating new PreviousExperience record for {model.Username}");
                    // Create new record - nullable strings don't need default values
                    model.ApplicantConfirmedAt = model.ApplicantConfirmed ? DateTime.UtcNow : null;
                    model.CreatedAt = DateTime.UtcNow;
                    model.UpdatedAt = DateTime.UtcNow;
                    
                    // Ensure collections are initialized
                    model.Qualifications = model.Qualifications ?? new List<Qualification>();
                    model.EmploymentHistory = model.EmploymentHistory ?? new List<EmploymentHistoryJob>();
                    model.ClinicalExperience = model.ClinicalExperience ?? new List<ClinicalProcedureEntry>();
                    
                    _context.PreviousExperiences.Add(model);
                    Console.WriteLine($"DASHBOARD DEBUG: Created new record for {model.Username}");
                }
                
                // Save to database - same pattern as PerformerDetails
                var savedRows = _context.SaveChanges();
                Console.WriteLine($"DASHBOARD DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                
                if (savedRows > 0)
                {
                    TempData["SuccessMessage"] = "Previous Experience information saved successfully!";
                    Console.WriteLine($"DASHBOARD DEBUG: PreviousExperience saved successfully to Railway PostgreSQL database for {model.Username}");
                    Console.WriteLine($"DASHBOARD DEBUG: GDC Gaps: {model.GdcGapsExplanation}");
                    Console.WriteLine($"DASHBOARD DEBUG: NHS Experience: {model.NhsExperience}");
                    Console.WriteLine($"DASHBOARD DEBUG: Qualifications count: {model.Qualifications?.Count ?? 0}");
                    Console.WriteLine($"DASHBOARD DEBUG: Employment History count: {model.EmploymentHistory?.Count ?? 0}");
                    Console.WriteLine($"DASHBOARD DEBUG: Clinical Experience count: {model.ClinicalExperience?.Count ?? 0}");
                }
                else
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Save failed - no rows affected for {model.Username}");
                    TempData["ErrorMessage"] = "Save failed - no changes were made to the database.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to save Previous Experience data: {ex.Message}");
                Console.WriteLine($"ERROR: Stack trace: {ex.StackTrace}");
                
                if (ex.Message.Contains("PreviousExperiences") && ex.Message.Contains("does not exist"))
                {
                    TempData["ErrorMessage"] = "The Previous Experience database table is being set up. Please try saving again in a few moments.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save Previous Experience information. Please try again.";
                }
            }
            
            // Redirect back to the form
            return RedirectToAction("PreviousExperience", new { performerUsername = model?.Username ?? currentUser });
        }

        // Simple Previous Experience Actions
        [HttpPost]
        public IActionResult SimplePreviousExperience(SimplePreviousExperienceModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            try
            {
                if (string.IsNullOrEmpty(model.Username))
                {
                    model.Username = currentUser ?? string.Empty;
                }

                Console.WriteLine($"DASHBOARD DEBUG: Attempting to save simple previous experience for {model.Username}");
                
                // Use existing PreviousExperiences table
                var existingRecord = _context.PreviousExperiences.FirstOrDefault(p => p.Username == model.Username);
                
                if (existingRecord != null)
                {
                    // Update existing record - map simple fields to existing complex model
                    existingRecord.NhsExperience = model.PreviousEmployer;
                    existingRecord.GdcGapsExplanation = model.JobDescription;
                    // Keep other fields as they were
                    
                    _context.PreviousExperiences.Update(existingRecord);
                    Console.WriteLine($"DASHBOARD DEBUG: Updated existing record for {model.Username}");
                }
                else
                {
                    // Create new record using existing PreviousExperienceModel
                    var newRecord = new PreviousExperienceModel
                    {
                        Username = model.Username,
                        NhsExperience = model.PreviousEmployer,
                        GdcGapsExplanation = model.JobDescription,
                        // Set other required fields to avoid validation issues
                        Years = "0",
                        Months = "0",
                        FullTime = "No"
                    };
                    
                    _context.PreviousExperiences.Add(newRecord);
                    Console.WriteLine($"DASHBOARD DEBUG: Created new record for {model.Username}");
                }

                var savedRows = _context.SaveChanges();
                Console.WriteLine($"DASHBOARD DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                
                if (savedRows > 0)
                {
                    TempData["SuccessMessage"] = "Previous Experience saved successfully!";
                    Console.WriteLine($"DASHBOARD DEBUG: Simple Previous Experience saved successfully for {model.Username}");
                }
                else
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Save failed - no rows affected for {model.Username}");
                    TempData["ErrorMessage"] = "Save failed - no changes were made.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to save Simple Previous Experience: {ex.Message}");
                Console.WriteLine($"ERROR: Stack trace: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Failed to save Previous Experience information. Please try again.";
            }
            
            return RedirectToAction("PreviousExperience", new { performerUsername = model.Username });
        }

        // Other section methods (simplified for brevity)
        public IActionResult StructuredConversation(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "StructuredConversation");
        }

        public IActionResult AgreementTerms(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "AgreementTerms");
        }

        public IActionResult WorkBasedAssessments(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "WorkBasedAssessments");
        }

        public IActionResult LearningModules(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "LearningModules");
        }

        public IActionResult LearningProgress(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "LearningProgress");
        }

        public IActionResult SupplementaryUploads(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "SupplementaryUploads");
        }

        public IActionResult ManagementPlan(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "ManagementPlan");
        }

        public IActionResult SkillsAssessment(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "SkillsAssessment");
        }

        public IActionResult CPD(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "CPD");
        }

        public IActionResult MSF(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "MSF");
        }

        public IActionResult PSQ(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "PSQ");
        }

        public IActionResult ClinicalNoteAudit(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "ClinicalNoteAudit");
        }

        public IActionResult HelpGuidance(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "HelpGuidance");
        }

        public IActionResult FinalSignOff(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "FinalSignOff");
        }

        private IActionResult HandlePerformerSection(string performerUsername, string sectionName)
        {
            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check permissions
            if (currentRole == "performer" && currentUser != performerUsername)
            {
                return RedirectToAction("Index");
            }

            // Set common ViewBag properties
            ViewBag.PerformerUsername = performerUsername;
            ViewBag.CurrentUserRole = currentRole;
            ViewBag.CurrentUser = currentUser;
            ViewBag.IsOwnDashboard = (currentUser == performerUsername);
            ViewBag.CanEdit = (currentRole == "performer" && currentUser == performerUsername);
            ViewBag.CanComment = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.CanApprove = (currentRole == "supervisor" || currentRole == "advisor" || currentRole == "superuser");
            ViewBag.IsReadOnly = (currentRole == "admin");
            ViewBag.ActiveSection = sectionName;

            // Get performer's name for display
            var performer3 = _context.Users.FirstOrDefault(u => u.Username == performerUsername);
            if (performer3 != null)
            {
                ViewBag.PerformerName = $"{performer3.FirstName} {performer3.LastName}";
            }

            return View($"Performer/{sectionName}");
        }
    }
}
