using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PetVaccination
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string VaccineType { get; set; } // Name of the vaccine

    [Required]
    public DateTime VaccineDate { get; set; } // Date of vaccination

    // **Foreign Key (Link to Pet)**
    [ForeignKey("Pet")]
    public int PetId { get; set; }
    public Pet Pet { get; set; } // Navigation Property
}
