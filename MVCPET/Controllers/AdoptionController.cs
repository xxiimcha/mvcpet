using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

public class AdoptionController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdoptionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult SubmitAdoption([FromBody] AdoptionRequest request)
    {
        if (request == null)
        {
            return Json(new { success = false, message = "No data received." });
        }

        // Fetch Pet and User objects manually from the database
        var pet = _context.Pets.FirstOrDefault(p => p.Id == request.PetId);
        var user = _context.Users.FirstOrDefault(u => u.Id == request.UserId);

        if (pet == null)
        {
            return Json(new { success = false, message = "Invalid PetId: Pet not found." });
        }

        if (user == null)
        {
            return Json(new { success = false, message = "Invalid UserId: User not found." });
        }

        // ✅ Assign Pet and User objects before saving
        request.Pet = pet;
        request.User = user;

        // Set default values for required fields
        request.Status = "Pending";
        request.RequestDate = DateTime.Now;

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return Json(new { success = false, message = "Validation errors", errors });
        }

        try
        {
            _context.AdoptionRequests.Add(request);
            _context.SaveChanges();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred: " + ex.Message });
        }
    }

}
