using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
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

        request.Pet = pet;
        request.User = user;
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

    [HttpPost]
    public IActionResult ApproveRequest(int id)
    {
        var request = _context.AdoptionRequests
                              .Include(r => r.Pet)  // Ensure the Pet is loaded
                              .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return Json(new { success = false, message = "Request not found." });
        }

        if (request.Pet == null)
        {
            return Json(new { success = false, message = "Pet associated with this request not found." });
        }

        // Update adoption request status
        request.Status = "Approved";

        // Mark the pet as adopted
        request.Pet.IsAdopted = true;

        _context.SaveChanges();

        return Json(new { success = true });
    }

    [HttpPost]
    public IActionResult RejectRequest(int id, [FromBody] JsonElement data)
    {
        var request = _context.AdoptionRequests.FirstOrDefault(r => r.Id == id);
        if (request == null)
        {
            return Json(new { success = false, message = "Request not found." });
        }

        if (!data.TryGetProperty("reason", out JsonElement reasonElement) || reasonElement.GetString() == null)
        {
            return Json(new { success = false, message = "Rejection reason is required." });
        }

        string rejectionReason = reasonElement.GetString();

        request.Status = "Rejected";
        request.RejectionReason = rejectionReason;
        _context.SaveChanges();

        return Json(new { success = true });
    }
}
