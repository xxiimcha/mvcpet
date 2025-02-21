using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCPET.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace MVCPET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Home";

            // Retrieve the logged-in user's name from the session
            string userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName; // Pass it to the view

            return View();
        }

        public async Task<IActionResult> Adopt()
        {
            ViewData["Title"] = "Adopt a Pet";

            var adoptees = await _context.Pets.Where(p => !p.IsAdopted).ToListAsync();
            var adoptedPets = await _context.Pets.Where(p => p.IsAdopted).ToListAsync();

            var model = new PetViewModel
            {
                Adoptees = adoptees,
                AdoptedPets = adoptedPets
            };

            return View(model);
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        public IActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            ViewData["Title"] = "Forgot Password";
            return View();
        }



        // Check if email exists
        [HttpPost] // Change from HttpGet to HttpPost
        public async Task<IActionResult> CheckEmail([FromBody] CheckEmailRequest request)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            return Json(new { exists = userExists });
        }

        // Create a simple DTO for JSON handling
        public class CheckEmailRequest
        {
            public string Email { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string email, string password, string NewPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("ChangePassword");
            }

            // Hash the entered current password
            string hashedCurrentPassword = HashPassword(password);

            // Validate current password against stored hashed password
            if (user.Password != hashedCurrentPassword)
            {
                TempData["Error"] = "Incorrect current password.";
                return RedirectToAction("ChangePassword");
            }

            // Hash the new password before saving
            user.Password = HashPassword(NewPassword);

            // Save the changes correctly
            _context.Users.Update(user); // Explicitly mark the user as updated
            await _context.SaveChangesAsync(); // Ensure changes are saved

            TempData["Success"] = "Password changed successfully.";
            return RedirectToAction("Login");
        }

        // Password Hashing Function (SHA-256)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Convert byte to hexadecimal
                }
                return builder.ToString();
            }
        }

        public IActionResult PetDetails(int id)
        {
            var pet = _context.Pets
                .Include(p => p.Vaccinations) // Ensure Vaccinations are included
                .FirstOrDefault(p => p.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        public IActionResult PetForm(int petId)
        {
            var pet = _context.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null)
            {
                return NotFound();
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ensure UserId is included in the model
            dynamic model = new
            {
                Pet = pet,
                UserId = user.Id,  
                UserName = user.Name,
                UserEmail = user.Email,
                UserAddress = user.Address
            };

            return View(model);
        }

        public IActionResult UserProfile()
        {
            ViewData["Title"] = "User Profile";

            // Get logged-in user ID from session
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not logged in
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Fetch the latest phone number from the AdoptionRequest table
            var latestAdoptionRequest = _context.AdoptionRequests
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.RequestDate) // Get the most recent request
                .FirstOrDefault();

            string phoneNumber = latestAdoptionRequest?.Phone ?? "N/A"; // Use "N/A" if no phone number is found

            // Fetch all adoption requests for the logged-in user
            var adoptionRequests = _context.AdoptionRequests
                .Where(a => a.UserId == userId)
                .Include(a => a.Pet) // Ensure Pet details are loaded
                .ToList();

            // Prepare the model
            var model = new AdoptionViewModel
            {
                User = user,
                PhoneNumber = phoneNumber, // Include phone number in the model
                AdoptionRequests = adoptionRequests
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public class PetViewModel
        {
            public List<Pet> Adoptees { get; set; }
            public List<Pet> AdoptedPets { get; set; }
        }

        // ViewModel for User Profile and Adoption Requests
        public class AdoptionViewModel
        {
            public User User { get; set; }
            public string PhoneNumber { get; set; } // Add phone number field
            public List<AdoptionRequest> AdoptionRequests { get; set; }
        }
    }
}
