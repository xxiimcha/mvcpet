using Microsoft.AspNetCore.Mvc;
using MVCPET.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVCPET.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new { success = false, message = "All fields are required." });
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return Json(new { success = false, message = "Email is already registered." });
            }

            // Hash the password before saving
            model.Password = HashPassword(model.Password);
            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Registration successful!" });
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new { success = false, message = "Email and Password are required." });
            }

            // Hash the entered password for security
            string hashedPassword = HashPassword(model.Password);

            // Check if user exists in the database
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == hashedPassword);

            if (user == null)
            {
                return Json(new { success = false, message = "Invalid Email or Password." });
            }

            try
            {
                // Store user session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserRole", user.Role);

                // Determine redirect URL based on role
                string role = user.Role?.ToLower(); // Handle potential null roles
                string redirectUrl = role switch
                {
                    "admin" => "/Admin/PetDetailAdmin",
                    "evaluator" => "/Evaluator/Dashboard",
                    "user" => "/Home/Index",
                    _ => "/Home/Index" // Default redirect
                };

                return Json(new { success = true, message = "Login successful!", redirectUrl = redirectUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Session Error: {ex.Message}");

                // Ensure the function always returns a value
                return Json(new { success = false, message = "An error occurred while logging in. Please try again." });
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet, HttpPost] // Allow both GET and POST requests
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Redirect to home page after logout
            return RedirectToAction("Index", "Home");
        }
    }
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
