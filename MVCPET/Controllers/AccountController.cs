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
    }
}
