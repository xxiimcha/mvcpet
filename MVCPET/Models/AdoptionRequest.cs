using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AdoptionRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PetId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("PetId")]
    public virtual Pet? Pet { get; set; }  // Allow null values

    [ForeignKey("UserId")]
    public virtual User? User { get; set; } // Allow null values

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public DateTime RequestDate { get; set; } = DateTime.Now;

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, MaxLength(500)]
    public string Address { get; set; }

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; }

    [Required, MaxLength(15)]
    public string Phone { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [MaxLength(100)]
    public string Occupation { get; set; }

    [Required, MaxLength(50)]
    public string MaritalStatus { get; set; }

    [Required, MaxLength(50)]
    public string Pronouns { get; set; }

    [Required]
    public bool HasAdoptedBefore { get; set; }

    [Required]
    public bool IsAdoptingForAnother { get; set; }

    [Required, MaxLength(50)]
    public string PetPreference { get; set; }

    [Required, MaxLength(50)]
    public string LivingType { get; set; }

    [Required]
    public bool IsRenting { get; set; }

    [Required, MaxLength(100)]
    public string HouseholdMembers { get; set; }

    [Required]
    public bool AgreesToInterview { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }
}
