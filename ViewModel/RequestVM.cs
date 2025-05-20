using Online_Medicine_Donation.DataModel;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;

namespace Online_Medicine_Donation.ViewModel
{
     public class RequestVM
     {
         public DonationRequest donationRequest { get; set; }
         public EmergencyRequest emergencyRequest { get; set; }

        public UserProfile userProfile { get; set; }


        public List<UserProfile> NgoList { get; set; }

        public WithdrawRequest withdrawRequest { get; set; }

        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DonationTime { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public string? Company { get; set; }
        public string? Type { get; set; }
        public string? Condition { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Status { get; set; }

        public IFormFile? PhotoFile { get; set; }

    }

}
