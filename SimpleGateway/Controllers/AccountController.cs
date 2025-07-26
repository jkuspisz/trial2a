using Microsoft.AspNetCore.Mvc;
using SimpleGateway.Models;
using SimpleGateway.Data;

namespace SimpleGateway.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.IsActive);
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
