using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCPET.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Home";
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

        public IActionResult PetDetails(int id)
        {
            var pet = _context.Pets.FirstOrDefault(p => p.Id == id);
            if (pet == null)
            {
                return NotFound(); // Handle missing pet gracefully
            }

            return View(pet); // Ensure the View receives a Pet model
        }


        public IActionResult UserProfile()
        {
            ViewData["Title"] = "User Profile";
            return View();
        }

        public IActionResult PetForm()
        {
            ViewData["Title"] = "Adoption Form";
            return View();
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
    }
}
