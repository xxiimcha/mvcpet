using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        if (pet == null || user == null)
        {
            return Json(new { success = false, message = "Invalid PetId or UserId." });
        }

        request.Pet = pet;
        request.User = user;
        request.Status = "Pending";
        request.RequestDate = DateTime.Now;

        try
        {
            _context.AdoptionRequests.Add(request);
            _context.SaveChanges();

            // Send Submission Email
            SendEmailNotification(user.Email, "Adoption Request Submitted",
                GetEmailTemplate("Your adoption request has been submitted.",
                $"Hello {user.Name},<br><br>" +
                $"We have received your adoption request for <b>{pet.Name}</b>. Our team will review it and get back to you soon.<br><br>" +
                $"Thank you for choosing to adopt!<br><br>Best regards,<br><b>Second Chances Team</b>"));

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
                              .Include(r => r.Pet)
                              .Include(r => r.User)
                              .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return Json(new { success = false, message = "Request not found." });
        }

        request.Status = "Approved";
        request.Pet.IsAdopted = true;
        _context.SaveChanges();

        // Send Approval Email
        SendEmailNotification(request.User.Email, "Adoption Request Approved",
            GetEmailTemplate("Your adoption request has been approved!",
            $"Hello {request.User.Name},<br><br>" +
            $"Congratulations! Your adoption request for <b>{request.Pet.Name}</b> has been approved. " +
            $"Please visit our adoption center to complete the final steps.<br><br>" +
            $"Best regards,<br><b>Second Chances Team</b>"));

        return Json(new { success = true });
    }

    [HttpPost]
    public IActionResult RejectRequest(int id, [FromBody] JsonElement data)
    {
        var request = _context.AdoptionRequests
                              .Include(r => r.User)
                              .FirstOrDefault(r => r.Id == id);

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

        // Send Rejection Email
        SendEmailNotification(request.User.Email, "Adoption Request Rejected",
            GetEmailTemplate("Your adoption request has been rejected.",
            $"Hello {request.User.Name},<br><br>" +
            $"We regret to inform you that your adoption request for <b>{request.Pet.Name}</b> has been rejected.<br><br>" +
            $"<b>Reason:</b> {rejectionReason}<br><br>" +
            $"For more details, please contact our support team.<br><br>" +
            $"Best regards,<br><b>Second Chances Team</b>"));

        return Json(new { success = true });
    }

    // 🔹 SMTP Email Notification Function
    private void SendEmailNotification(string toEmail, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com") // Using Gmail SMTP
            {
                Port = 587,
                Credentials = new NetworkCredential("marklegend029@gmail.com", "cytf tuev scvc okyi"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("marklegend029@gmail.com", "Second Chances Team"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email Error: {ex.Message}");
        }
    }

    // 🔹 Email Template for Better Design
    private string GetEmailTemplate(string title, string message)
    {
        return $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
            <div style='text-align: center;'>
                <h2 style='color: #2d4c3b;'>🐾 Second Chances 🐾</h2>
                <h3 style='color: #333;'>{title}</h3>
            </div>
            <div style='padding: 15px; font-size: 16px; color: #555;'>
                {message}
            </div>
            <div style='text-align: center; padding-top: 20px;'>
                <a href='https://yourwebsite.com' style='text-decoration: none; background-color: #2d4c3b; color: white; padding: 10px 20px; border-radius: 5px;'>Visit Our Website</a>
            </div>
            <div style='text-align: center; margin-top: 20px; font-size: 12px; color: #777;'>
                © {DateTime.Now.Year} Second Chances. All rights reserved.
            </div>
        </div>";
    }
}
