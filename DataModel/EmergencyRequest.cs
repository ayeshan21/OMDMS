using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.DataModel
{
    public class EmergencyRequest
    {
        [Key]
        public int? Id { get; set; }

        public Guid? EmergencyId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "NGO Name")]
        public string? NgoName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Medicine Full Name")]
        public string? MedicineName { get; set; }

        [Required]
        [Display(Name = "Medicine Quantity")]
        public int? Quantity { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Medicine Type")]
        public string? Type { get; set; }
        public string? Status { get; set; }
    }
}
