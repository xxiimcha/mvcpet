using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; }

    [Required, MaxLength(255)]
    public string Password { get; set; } // Ensure to hash passwords before storing

    [MaxLength(500)]
    public string Address { get; set; }

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = "User"; // Default role is "User", can be "Admin"

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
