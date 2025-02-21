using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pet
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Age { get; set; }
    public string Species { get; set; }
    public string Temperament { get; set; }
    public string Color { get; set; }
    public decimal Weight { get; set; }
    public bool Vaccinated { get; set; }
    public string Description { get; set; }
    public string PhotoPath { get; set; }
    public bool IsAdopted { get; set; }
    public bool Neuter { get; set; }
    public string Diagnosis { get; set; }

    // 🔹 Added Sex Field (Male/Female)
    [Required]
    public string Sex { get; set; }  // Example values: "Male", "Female"

    // Foreign Key for Adoption Request
    public int? AdoptionRequestId { get; set; } // Make it nullable to avoid errors
    [ForeignKey("AdoptionRequestId")]
    public virtual AdoptionRequest AdoptionRequest { get; set; }

    // Relationship to Vaccination Table
    public List<PetVaccination> Vaccinations { get; set; } = new List<PetVaccination>();
}
