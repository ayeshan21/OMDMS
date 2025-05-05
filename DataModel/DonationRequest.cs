using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.DataModel
{
    public class DonationRequest
    {
        [Key]
        public int? Id { get; set; }

        public Guid? DonationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Medicine Full Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Medicine Quantity")]
        public int? Quantity { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Pharmaceutical Company Name")]
        public string? Company { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Medicine Type")]
        public string? Type { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Required]
        [Display(Name = "Medicine Condition")]
        [StringLength(30)]
        public string? Condition { get; set; }

        [Required]
        [Display(Name = "Select NGO")]
        [StringLength(30)]
        public string? SelectNgo { get; set; }

        [Required]
        [Display(Name = "Medicine Photo URL")]
        public string? MedicinePhotoUrl { get; set; }

    }
}

