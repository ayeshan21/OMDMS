using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Medicine_Donation.ViewModel
{
    public class MedicineInventoryVM
    {
        [Key]
        public int? Id { get; set; }

        public Guid? UserId { get; set; }

        public int DonationRequestsId { get; set; }
        public string? Name { get; set; }

        public int? Quantity { get; set; }

        public string? Company { get; set; }

        public string? Type { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? Condition { get; set; }

        public string? Status { get; set; }

        public string? MedicinePhotoUrl { get; set; }
        public IFormFile? PhotoFile { get; set; }
 
    }
}
