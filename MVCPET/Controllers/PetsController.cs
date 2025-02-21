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
        [Route("Pets/AddPets")]
        public async Task<IActionResult> AddPets(
    [FromForm] Pet pet,
    IFormFile Photo,
    [FromForm] List<string> VaccineType,
    [FromForm] List<DateTime> VaccineDate)
        {
            if (string.IsNullOrEmpty(pet.Name) || string.IsNullOrEmpty(pet.Species))
            {
                return Json(new { success = false, message = "Required fields are missing." });
            }

            // Debugging: Log received vaccine data
            Console.WriteLine($"Received {VaccineType.Count} vaccines for {pet.Name}");
            for (int i = 0; i < VaccineType.Count; i++)
            {
                Console.WriteLine($"Vaccine {i + 1}: {VaccineType[i]} - {VaccineDate[i]}");
            }

            // Handle Photo Upload
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

            // Add pet to database first
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();  // Ensure pet ID is generated

            Console.WriteLine($"New Pet ID: {pet.Id}"); // Debugging

            // Ensure vaccine data exists and has the same count
            if (VaccineType != null && VaccineDate != null && VaccineType.Count == VaccineDate.Count)
            {
                var vaccineList = new List<PetVaccination>();

                for (int i = 0; i < VaccineType.Count; i++)
                {
                    var petVaccine = new PetVaccination
                    {
                        PetId = pet.Id,  // Link vaccine to pet
                        VaccineType = VaccineType[i],
                        VaccineDate = VaccineDate[i]
                    };

                    vaccineList.Add(petVaccine);
                }

                _context.Vaccinations.AddRange(vaccineList);
                await _context.SaveChangesAsync();
                Console.WriteLine("Vaccination records added successfully."); // Debugging
            }
            else
            {
                Console.WriteLine("Vaccine data is missing or mismatched.");
            }

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
        public async Task<IActionResult> Delete(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
                return Json(new { success = false });

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
