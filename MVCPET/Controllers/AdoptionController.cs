using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdoptionController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdoptionController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Apply(int petId)
    {
        // Fetch pet details
        var pet = _context.Pets.FirstOrDefault(p => p.Id == petId);

        if (pet == null)
        {
            return NotFound("Pet not found.");
        }

        // Create a new adoption request model
        var adoptionRequest = new AdoptionRequest
        {
            PetId = pet.Id,
            Pet = pet // Assign pet details
        };

        return View("PetForm", adoptionRequest); // Ensure the view is correctly named
    }
}
