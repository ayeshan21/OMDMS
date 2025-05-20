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
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
   
    [Area("Admin"), Route("Admin")]
    //[Authorize(Roles = "Admin")]
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
            var medicineCount = _context.MedicineInventories.Count();

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
                    Id=m.Id,
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
                    Id=m.Id,
                    EmergencyId = m.EmergencyId, // Use existing ID
                    NgoName = m.NgoName,
                    MedicineName = m.MedicineName,
                    Quantity = m.Quantity,
                    Type = m.Type,
                    Reason = m.Reason,
                    Days = m.Days,
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
        public IActionResult AcceptRequest(int EmergencyId)
        {
            var emergencyrequest = _context.EmergencyRequests.Where(x=>x.Id == EmergencyId).FirstOrDefault();

            if (emergencyrequest != null)
            {
                emergencyrequest.Status = "Accepted";
                _context.SaveChanges();
            }
        
            return RedirectToAction("Tables");
        }

        [Route("RejectRequest")]
        [HttpPost]
        public IActionResult RejectRequest(int EmergencyId)
        {
            var emergencyrequest = _context.EmergencyRequests.Where(x => x.Id == EmergencyId).FirstOrDefault();

            if (emergencyrequest != null)
            {
                emergencyrequest.Status = "Rejected";
                _context.SaveChanges();
            }
          
            return RedirectToAction("Tables");
        }


        [Route("MedicineDetails")]
        [HttpGet]
        public IActionResult MedicineDetails(int Id)
        {
            var donation = _context.DonationRequests.FirstOrDefault(d => d.Id == Id);
            var specificDonor = _context.UserProfiles.FirstOrDefault(u => u.Role == "Donor"); 


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

        [Route("DonatedMedicine")]
       
        public IActionResult DonatedMedicine()
        {
            var medicineList = (from donation in _context.DonationRequests    
                                join userProfile in _context.UserProfiles
                                    on donation.DonationId equals userProfile.UserId
                                select new RequestVM
                                {
                                        Name = donation.Name,
                                        Quantity = donation.Quantity,
                                        Type = donation.Type,
                                        Condition = donation.Condition,
                                        ExpiryDate = donation.ExpiryDate,
                                        DonationTime = donation.DonationTime,
                                        FullName = userProfile.FullName,
                                        Address = userProfile.Address,
                                        PhoneNumber = userProfile.PhoneNumber,
                                        Status = donation.Status   
                                }).ToList();

            var viewModel = new CombinedRequestVM
            {
                DonationRequests = medicineList
            };

            return View(viewModel);
        }

        
    }
}
