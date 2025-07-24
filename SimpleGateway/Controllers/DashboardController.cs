using Microsoft.AspNetCore.Mvc;
using SimpleGateway.Models;

namespace SimpleGateway.Controllers
{
    public class DashboardController : Controller
    {
        // Static storage for performer details (in production, this would be a database)
        private static Dictionary<string, PerformerDetailsModel> performerDetailsStore = new();
        
        // Static storage for user data (in production, this would be a database)
        private static Dictionary<string, UserModel> userStore = new()
        {
            {"performer1", new UserModel { Username = "performer1", Password = "password123", Role = "performer", FirstName = "John", LastName = "Smith" }},
            {"performer2", new UserModel { Username = "performer2", Password = "password123", Role = "performer", FirstName = "Jane", LastName = "Johnson" }},
            {"performer3", new UserModel { Username = "performer3", Password = "password123", Role = "performer", FirstName = "Mike", LastName = "Wilson" }},
            {"advisor1", new UserModel { Username = "advisor1", Password = "password123", Role = "advisor", FirstName = "Dr. Sarah", LastName = "Davis" }},
            {"supervisor1", new UserModel { Username = "supervisor1", Password = "password123", Role = "supervisor", FirstName = "Prof. Robert", LastName = "Brown" }},
            {"admin1", new UserModel { Username = "admin1", Password = "password123", Role = "admin", FirstName = "Admin", LastName = "User" }}
        };

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
            else
            {
                // Show list of performers for advisors, supervisors, and admins
                ViewBag.Users = userStore.Values.Where(u => u.Role == "performer").ToList();
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
            if (userStore.ContainsKey(performerUsername))
            {
                ViewBag.PerformerName = $"{userStore[performerUsername].FirstName} {userStore[performerUsername].LastName}";
            }

            // Get or create performer details
            var model = performerDetailsStore.GetValueOrDefault(performerUsername) ?? new PerformerDetailsModel
            {
                Username = performerUsername
            };

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
                // Store the model
                performerDetailsStore[model.Username] = model;
                TempData["SuccessMessage"] = "Details saved successfully!";
                return RedirectToAction("PerformerDetails", new { performerUsername = model.Username });
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PerformerUsername = model.Username;
            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentUserRole = HttpContext.Session.GetString("role");
            ViewBag.IsOwnDashboard = (currentUser == model.Username);
            ViewBag.ActiveSection = "PerformerDetails";
            
            if (userStore.ContainsKey(model.Username))
            {
                ViewBag.PerformerName = $"{userStore[model.Username].FirstName} {userStore[model.Username].LastName}";
            }
            
            return View("Performer/PerformerDetails", model);
        }

        public IActionResult Uploads(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "Uploads");
        }

        public IActionResult PreviousExperienceForm(string performerUsername)
        {
            return HandlePerformerSection(performerUsername, "PreviousExperience");
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
            if (userStore.ContainsKey(performerUsername))
            {
                ViewBag.PerformerName = $"{userStore[performerUsername].FirstName} {userStore[performerUsername].LastName}";
            }

            return View($"Performer/{sectionName}");
        }
    }
}
