using Microsoft.AspNetCore.Mvc;
using MVCPET.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public ActionResult AddUser(User newUser)
    {
        if (ModelState.IsValid)
        {
            // Hash the password before storing it
            newUser.Password = HashPassword(newUser.Password);
            newUser.CreatedAt = DateTime.Now;

            _context.Users.Add(newUser);
            _context.SaveChanges(); // Save to database

            // Redirect to Admin/UserManagement
            return RedirectToAction("UserManagement", "Admin");
        }

        // If validation fails, reload the UserManagement view with current users
        return View("UserManagement", _context.Users.ToList());
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
