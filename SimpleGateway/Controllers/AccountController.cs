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
            Console.WriteLine($"=== LOGIN ATTEMPT ===");
            Console.WriteLine($"Username: {username}");
            Console.WriteLine($"Session ID: {HttpContext.Session.Id}");
            
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.IsActive);
            if (user != null)
            {
                // Save username, displayName, and role in session
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("displayName", user.DisplayName);
                HttpContext.Session.SetString("role", user.Role);
                
                Console.WriteLine($"✅ Login successful for {user.Username}");
                Console.WriteLine($"Session variables set:");
                Console.WriteLine($"  - username: {HttpContext.Session.GetString("username")}");
                Console.WriteLine($"  - displayName: {HttpContext.Session.GetString("displayName")}");
                Console.WriteLine($"  - role: {HttpContext.Session.GetString("role")}");
                
                return RedirectToAction("Index", "Dashboard");
            }
            
            Console.WriteLine($"❌ Login failed for {username}");
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
