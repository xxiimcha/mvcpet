using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
        var request = _context.AdoptionRequests.FirstOrDefault(r => r.Id == id);
        if (request == null)
        {
            return Json(new { success = false, message = "Request not found." });
        }

        request.Status = "Approved";
        _context.SaveChanges();

        return Json(new { success = true, message = "Adoption request approved." });
    }

    [HttpPost]
    public IActionResult RejectRequest(int id, string reason)
    {
        var request = _context.AdoptionRequests.FirstOrDefault(r => r.Id == id);
        if (request == null)
        {
            return Json(new { success = false, message = "Request not found." });
        }

        request.Status = "Rejected";
        request.RejectionReason = reason;
        _context.SaveChanges();

        return Json(new { success = true, message = "Adoption request rejected." });
    }
}
