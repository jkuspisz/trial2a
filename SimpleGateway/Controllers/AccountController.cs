using Microsoft.AspNetCore.Mvc;
using SimpleGateway.Models;

namespace SimpleGateway.Controllers
{
    public class AccountController : Controller
    {
        // Hardcoded users
        private static List<UserModel> demoUsers = new()
        {
            new UserModel { Username = "performer1", Password = "test123", DisplayName = "John Smith", Role = "performer" },
            new UserModel { Username = "performer2", Password = "test123", DisplayName = "Sarah Johnson", Role = "performer" },
            new UserModel { Username = "performer3", Password = "test123", DisplayName = "Mike Wilson", Role = "performer" },
            new UserModel { Username = "superuser", Password = "test123", DisplayName = "Super User", Role = "superuser" },
            new UserModel { Username = "admin", Password = "admin123", DisplayName = "Administrator", Role = "admin" },
            new UserModel { Username = "supervisor", Password = "super123", DisplayName = "Dr. Emma Thompson", Role = "supervisor" },
            new UserModel { Username = "advisor", Password = "advise123", DisplayName = "Prof. David Brown", Role = "advisor" }
        };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = demoUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Save username, displayName, and role in session
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("displayName", user.DisplayName);
                HttpContext.Session.SetString("role", user.Role);
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error = "Invalid login";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
