using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.ViewModel
{
    public class WithdrawRequestVM
    {
        [Key]
        public int? Id { get; set; }

        public Guid? NgoId { get; set; }

        public string? NgoName { get; set; }

        public string? MedicineName { get; set; }

        public int? Quantity { get; set; }

        public DateTime? WithdrawTime { get; set; }
        public string? Purpose { get; set; }
    }
}
