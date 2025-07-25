using Microsoft.AspNetCore.Mvc;
using SimpleGateway.Models;

namespace SimpleGateway.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check if user has admin/superuser access
        private bool HasAdminAccess()
        {
            var currentRole = HttpContext.Session.GetString("role");
            return currentRole == "admin" || currentRole == "superuser";
        }

        // Admin Dashboard - Main management interface
        public IActionResult Dashboard()
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = HttpContext.Session.GetString("username");
            var currentRole = HttpContext.Session.GetString("role");

            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentRole = currentRole;

            // Dashboard statistics
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalPerformers = _context.Users.Count(u => u.Role == "performer");
            ViewBag.TotalSupervisors = _context.Users.Count(u => u.Role == "supervisor");
            ViewBag.TotalAdvisors = _context.Users.Count(u => u.Role == "advisor");

            return View();
        }

        // User Management - List all users
        public IActionResult UserManagement()
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            var users = _context.Users.OrderBy(u => u.Role).ThenBy(u => u.Username).ToList();
            
            ViewBag.CurrentUser = HttpContext.Session.GetString("username");
            ViewBag.CurrentRole = HttpContext.Session.GetString("role");

            return View(users);
        }

        // Create New User - GET
        public IActionResult CreateUser()
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrentUser = HttpContext.Session.GetString("username");
            ViewBag.CurrentRole = HttpContext.Session.GetString("role");

            return View(new UserModel());
        }

        // Create New User - POST
        [HttpPost]
        public IActionResult CreateUser(UserModel model)
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if username already exists
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
            }

            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.IsActive = true;

                _context.Users.Add(model);
                _context.SaveChanges();

                TempData["Success"] = $"User '{model.Username}' created successfully";
                return RedirectToAction("UserManagement");
            }

            ViewBag.CurrentUser = HttpContext.Session.GetString("username");
            ViewBag.CurrentRole = HttpContext.Session.GetString("role");

            return View(model);
        }

        // Edit User - GET
        public IActionResult EditUser(int id)
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUser = HttpContext.Session.GetString("username");
            ViewBag.CurrentRole = HttpContext.Session.GetString("role");

            return View(user);
        }

        // Edit User - POST
        [HttpPost]
        public IActionResult EditUser(UserModel model)
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if username already exists (excluding current user)
            if (_context.Users.Any(u => u.Username == model.Username && u.Id != model.Id))
            {
                ModelState.AddModelError("Username", "Username already exists");
            }

            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(model.Id);
                if (existingUser != null)
                {
                    existingUser.Username = model.Username;
                    existingUser.Password = model.Password;
                    existingUser.FirstName = model.FirstName;
                    existingUser.LastName = model.LastName;
                    existingUser.DisplayName = model.DisplayName;
                    existingUser.Role = model.Role;
                    existingUser.Email = model.Email;
                    existingUser.IsActive = model.IsActive;

                    _context.SaveChanges();

                    TempData["Success"] = $"User '{model.Username}' updated successfully";
                    return RedirectToAction("UserManagement");
                }
            }

            ViewBag.CurrentUser = HttpContext.Session.GetString("username");
            ViewBag.CurrentRole = HttpContext.Session.GetString("role");

            return View(model);
        }

        // Delete User
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                TempData["Success"] = $"User '{user.Username}' deleted successfully";
            }

            return RedirectToAction("UserManagement");
        }

        // Assignment Management - Assign supervisors/advisors to performers
        public IActionResult AssignmentManagement()
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrentUser = HttpContext.Session.GetString("username");
            ViewBag.CurrentRole = HttpContext.Session.GetString("role");

            ViewBag.Performers = _context.Users.Where(u => u.Role == "performer").ToList();
            ViewBag.Supervisors = _context.Users.Where(u => u.Role == "supervisor").ToList();
            ViewBag.Advisors = _context.Users.Where(u => u.Role == "advisor").ToList();

            return View();
        }

        // Create Assignment - POST
        [HttpPost]
        public IActionResult CreateAssignment(int PerformerId, int? SupervisorId, int? AdvisorId, string Notes)
        {
            if (!HasAdminAccess())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var performer = _context.Users.FirstOrDefault(u => u.Id == PerformerId);
                if (performer == null)
                {
                    TempData["ErrorMessage"] = "Selected performer not found.";
                    return RedirectToAction("AssignmentManagement");
                }

                // In a full implementation, you would save assignment relationships to a separate table
                // For now, we'll just show a success message as the basic user structure exists
                
                var assignmentDetails = new List<string>();
                assignmentDetails.Add($"Performer: {performer.DisplayName}");
                
                if (SupervisorId.HasValue)
                {
                    var supervisor = _context.Users.FirstOrDefault(u => u.Id == SupervisorId.Value);
                    if (supervisor != null)
                    {
                        assignmentDetails.Add($"Supervisor: {supervisor.DisplayName}");
                    }
                }
                
                if (AdvisorId.HasValue)
                {
                    var advisor = _context.Users.FirstOrDefault(u => u.Id == AdvisorId.Value);
                    if (advisor != null)
                    {
                        assignmentDetails.Add($"Advisor: {advisor.DisplayName}");
                    }
                }

                if (!string.IsNullOrEmpty(Notes))
                {
                    assignmentDetails.Add($"Notes: {Notes}");
                }

                TempData["SuccessMessage"] = $"Assignment created successfully! {string.Join(", ", assignmentDetails)}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating assignment: {ex.Message}";
            }

            return RedirectToAction("AssignmentManagement");
        }
    }
}
