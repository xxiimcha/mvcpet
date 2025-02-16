using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MVCPET.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult EvaluatorsDashboard()
        {
            return View(); // Renders Views/Admin/EvaluatorDashboard.cshtml
        }

        public IActionResult AdoptionRequestAdmin()
        {
            return View();
        }

        public IActionResult AllEmployeeAdmin()
        {
            return View();
        }

        public IActionResult CurrentEmployeeAdmin()
        {
            return View();
        }

        public IActionResult PastEmployeeAdmin()
        {
            return View();
        }

        public IActionResult AddEmployeeAdmin()
        {
            return View();
        }

        public async Task<IActionResult> PetDetailAdmin()
        {
            var totalPets = await _context.Pets.CountAsync();
            var adoptedPets = await _context.Pets.CountAsync(p => p.IsAdopted);
            var totalAccounts = 0; // If you have a users table

            ViewData["TotalPets"] = totalPets;
            ViewData["AdoptedPets"] = adoptedPets;
            ViewData["TotalAccounts"] = totalAccounts; // Adjust based on your DB structure

            return View(await _context.Pets.ToListAsync());
        }


        public IActionResult AdoptionAdmin()
        {
            return View();
        }

        public async Task<IActionResult> AdoptedPetsAdmin()
        {
            var adoptedPets = await _context.Pets
                                            .Where(p => p.IsAdopted)
                                            .ToListAsync();

            return View(adoptedPets);
        }

        public IActionResult AddPetsAdmin()
        {
            return View();
        }
    }
}
