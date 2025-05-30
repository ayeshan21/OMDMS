﻿using Online_Medicine_Donation.DataModel;
using Online_Medicine_Donation.ViewModel;

namespace Online_Medicine_Donation.ViewModel
{
    public class CombinedRequestVM
    {
        public List<RequestVM> DonationRequests { get; set; }
        public List<RequestVM> EmergencyRequests { get; set; }
        public List<RequestVM> MedicineInventorys { get; set; }
        public List<RequestVM> WithdrawRequests { get; set; }  
      
    }
}
 