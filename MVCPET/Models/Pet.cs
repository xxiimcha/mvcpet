using System.ComponentModel.DataAnnotations;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Species { get; set; }
    public string Temperament { get; set; }
    public string Color { get; set; }
    public decimal Weight { get; set; }
    public bool Vaccinated { get; set; }
    public string Description { get; set; }
    public string PhotoPath { get; set; }

    // New Column
    public bool IsAdopted { get; set; } // True = Adopted, False = Available
}
