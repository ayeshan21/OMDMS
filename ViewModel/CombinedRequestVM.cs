using Online_Medicine_Donation.ViewModel;

namespace Online_Medicine_Donation.ViewModel
{
    public class CombinedRequestVM
    {
        public List<RequestVM> DonationRequests { get; set; }
        public List<RequestVM> EmergencyRequests { get; set; }
    }
}
 