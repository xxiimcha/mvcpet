using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AdoptionRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Pet")]
    public int PetId { get; set; }
    public Pet Pet { get; set; } // Navigation property

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; } // Navigation property

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Default status

    public DateTime RequestDate { get; set; } = DateTime.Now;
}
