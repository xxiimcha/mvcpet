using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace MVCPET.Controllers
{
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPets([FromForm] Pet pet, IFormFile Photo)
        {
            if (string.IsNullOrEmpty(pet.Name) || string.IsNullOrEmpty(pet.Species))
            {
                return Json(new { success = false, message = "Required fields are missing." });
            }

            if (Photo != null && Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(fileStream);
                }

                pet.PhotoPath = "/uploads/" + fileName;
            }

            pet.IsAdopted = false; // Default value

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Pet added successfully!" });
        }


        [HttpPost]
        public async Task<IActionResult> UnadoptPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return Json(new { success = false, message = "Pet not found." });
            }

            pet.IsAdopted = false;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Pet unadopted successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return Json(new { success = false, message = "Pet not found." });
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Pet deleted successfully." });
        }
    }
}
