using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.ViewModel
{
    public class MedicineVM
    {
        [Key]
        public int? Id { get; set; }

        public Guid? MedicineId { get; set; }

        public string? Name { get; set; }

        public int? Quantity { get; set; }

        public string? Company { get; set; }

        public string? Type { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? Condition { get; set; }

        public string? MedicinePhotoUrl { get; set; }
        public IFormFile? PhotoFile { get; set; }

    }
}
