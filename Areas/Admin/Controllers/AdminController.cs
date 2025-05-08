using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Medicine_Donation.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using Online_Medicine_Donation.Controllers;
using Online_Medicine_Donation.ViewModel;
using Online_Medicine_Donation.DataModel;
using System.Net.Mail;
using System.Net;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
   
    [Area("Admin"), Route("Admin")]

    public class AdminController : BaseController

    {
        private readonly OnlineMedicineContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(OnlineMedicineContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            var ngoCount = _context.UserProfiles.Where(x => x.Role == "NGO").Count();
            var donorCount = _context.UserProfiles.Where(x => x.Role == "Donor").Count();
            var medicineCount = _context.Medicines.Count();

            ViewBag.NgoCount = ngoCount;
            ViewBag.DonorCount = donorCount;
            ViewBag.MedicineCount = medicineCount;
            return View();
        }

        [Route("NgoList")]
        public IActionResult NgoList()
        {
            var ngoProfiles = _context.UserProfiles.Where(x => x.Role == "NGO").ToList();

            return View(ngoProfiles);
        }
        [Route("DonorList")]
        public IActionResult DonorList()
        {
            var DonorProfiles = _context.UserProfiles.Where(x => x.Role == "Donor").ToList();

            return View(DonorProfiles);
        }
        [Route("UserList")]
        public IActionResult UserList()
        {
            var profileUserIds = _context.UserProfiles.Select(x => x.UserId).ToHashSet();

            var allUsers = _context.Users.ToList();

            var datalist = allUsers.Where(user => Guid.TryParse(user.Id, out Guid parsedId) && !profileUserIds.Contains(parsedId)).ToList();

            return View(datalist);
        }

        [Route("Tables")]
        public IActionResult Tables()
        {
            var donationrequest = _context.DonationRequests.Select(m => new RequestVM
            {
                donationRequest = new DonationRequest
                {
                    DonationId = m.DonationId, // Use existing ID
                    Name = m.Name,
                    Quantity = m.Quantity,
                    Type = m.Type,
                    Status = m.Status
                }
            }).ToList();

            var emergencyrequest = _context.EmergencyRequests.Select(m => new RequestVM
            {
                emergencyRequest = new EmergencyRequest
                {
                    EmergencyId = m.EmergencyId, // Use existing ID
                    NgoName = m.NgoName,
                    MedicineName = m.MedicineName,
                    Quantity = m.Quantity,
                    Type = m.Type,
                    Reason = m.Reason,
                    Status = m.Status
                }
            }).ToList();

            var viewModel = new CombinedRequestVM
            {
                DonationRequests = donationrequest,
                EmergencyRequests = emergencyrequest
            };

            return View(viewModel);
        }
        [HttpPost]
        [Route("AcceptRequest")]
        public IActionResult AcceptRequest(Guid EmergencyId)
        {
            var emergencyrequest = _context.EmergencyRequests.Where(x=>x.EmergencyId == EmergencyId).FirstOrDefault();

            if (emergencyrequest != null)
            {
                emergencyrequest.Status = "Accepted";
                _context.SaveChanges();
            }
        
            return RedirectToAction("Tables");
        }

        [Route("RejectRequest")]
        [HttpPost]
        public IActionResult RejectRequest(Guid EmergencyId)
        {
            var emergencyrequest = _context.EmergencyRequests.Where(x => x.EmergencyId == EmergencyId).FirstOrDefault();

            if (emergencyrequest != null)
            {
                emergencyrequest.Status = "Rejected";
                _context.SaveChanges();
            }
          
            return RedirectToAction("Tables");
        }


        [Route("MedicineDetails")]
        [HttpGet]
        public IActionResult MedicineDetails(Guid DonationId)
        {
            var donation = _context.DonationRequests.FirstOrDefault(d => d.DonationId == DonationId);
            var specificDonor = _context.UserProfiles.FirstOrDefault(u => u.Role == "Donor" && u.UserId == DonationId); 


            if (donation == null)
            {
                return NotFound(); 
            }

            var viewModel = new RequestVM
            {
                donationRequest = donation,
                userProfile = specificDonor
            };

            return View(viewModel); 
        }
    }
}
