namespace MVCPET.Models
{
    public class PetAdoptionViewModel
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Species { get; set; }
        public string PhotoPath { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerPhone { get; set; }
        public DateTime? AdoptionDate { get; set; }
    }
}
