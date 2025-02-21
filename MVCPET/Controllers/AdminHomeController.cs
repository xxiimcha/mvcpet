using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPET.Models;
using System;
using System.Linq;
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

        public async Task<IActionResult> EvaluatorsDashboard()
        {
            var adoptionRequests = await _context.AdoptionRequests
                .Include(ar => ar.Pet)   // Ensure pet data is loaded
                .ToListAsync();

            return View(adoptionRequests);
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
        public IActionResult AdoptedPetsAdmin()
        {
            var adoptedPets = _context.Pets
                .Where(p => p.IsAdopted)
                .Select(p => new PetAdoptionViewModel
                {
                    PetId = p.Id,
                    Name = p.Name,
                    Color = p.Color,
                    Species = p.Species,
                    PhotoPath = p.PhotoPath,
                    OwnerName = _context.AdoptionRequests
                        .Where(a => a.PetId == p.Id && a.Status == "Approved")
                        .Select(a => a.Name)
                        .FirstOrDefault() ?? "Not Available",
                    OwnerEmail = _context.AdoptionRequests
                        .Where(a => a.PetId == p.Id && a.Status == "Approved")
                        .Select(a => a.Email)
                        .FirstOrDefault() ?? "Not Available",
                    OwnerPhone = _context.AdoptionRequests
                        .Where(a => a.PetId == p.Id && a.Status == "Approved")
                        .Select(a => a.Phone)
                        .FirstOrDefault() ?? "Not Available",
                    AdoptionDate = _context.AdoptionRequests
                        .Where(a => a.PetId == p.Id && a.Status == "Approved")
                        .Select(a => (DateTime?)a.RequestDate)
                        .FirstOrDefault()
                })
                .ToList();

            return View(adoptedPets);
        }

        public IActionResult AddPetsAdmin()
        {
            return View();
        }
    }
}
