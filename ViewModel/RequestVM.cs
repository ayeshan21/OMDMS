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
        public IFormFile? PhotoFile { get; set; }

    }

}
