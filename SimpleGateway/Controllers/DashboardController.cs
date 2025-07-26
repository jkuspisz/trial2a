using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;

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
                // Redirect performers to their own dashboard
                return RedirectToAction("PerformerDetails", new { performerUsername = currentUser });
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
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Check if performer details already exists
                var existingDetails = _context.PerformerDetails.FirstOrDefault(p => p.Username == model.Username);
                
                if (existingDetails != null)
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Updating existing performer details for {model.Username}");
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
                }
                else
                {
                    Console.WriteLine($"DASHBOARD DEBUG: Creating new performer details for {model.Username}");
                    // Add new record
                    _context.PerformerDetails.Add(model);
                }
                
                var savedRows = _context.SaveChanges();
                Console.WriteLine($"DASHBOARD DEBUG: SaveChanges() affected {savedRows} rows for {model.Username}");
                TempData["SuccessMessage"] = "Details saved successfully!";
                return RedirectToAction("PerformerDetails", new { performerUsername = model.Username });
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
            return HandlePerformerSection(performerUsername, "PreviousExperience");
        }

        public IActionResult PreviousExperienceForm(string performerUsername)
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

            // Initialize empty previous experience model (you can load from database later)
            ViewBag.PreviousExperience = new PreviousExperienceModel { Username = performerUsername };
            
            // Initialize clinical entries as empty list for now
            ViewBag.ClinicalEntries = new List<ClinicalExperienceEntry>();
            
            // Initialize procedure sections from static data
            ViewBag.ProcedureSections = ProcedureSections.All;

            return View("Performer/PreviousExperienceForm");
        }

        [HttpPost]
        public IActionResult PreviousExperienceForm(PreviousExperienceModel model)
        {
            var currentUser = HttpContext.Session.GetString("username");
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // For now, just show a success message since we haven't implemented database storage yet
            TempData["SuccessMessage"] = "Previous Experience information noted. Database storage will be implemented soon.";
            
            Console.WriteLine($"DASHBOARD DEBUG: PreviousExperience POST received for {model.Username}");
            Console.WriteLine($"DASHBOARD DEBUG: GDC Gaps: {model.GdcGapsExplanation}");
            Console.WriteLine($"DASHBOARD DEBUG: NHS Experience: {model.NhsExperience}");
            
            // Redirect back to the form
            return RedirectToAction("PreviousExperienceForm", new { performerUsername = model.Username });
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
