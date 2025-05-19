using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.DataModel
{
    public class WithdrawRequest
    {
        [Key]

        public int? Id { get; set; }

        public Guid? NgoId { get; set; }

        [Required]
        [StringLength(50)]
        public string? NgoName { get; set; }

        [Required]
        [StringLength(100)]
        public string? MedicineName { get; set; }

        [Required]
        public int? Quantity { get; set; }

        [Required]
        public DateTime? WithdrawTime { get; set; }

        [Required]
        [StringLength(100)]
        public string? Purpose { get; set; }


    }
}
