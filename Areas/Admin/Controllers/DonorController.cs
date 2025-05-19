using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Medicine_Donation.Controllers;
using Online_Medicine_Donation.Data;
using Online_Medicine_Donation.DataModel;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;
using Online_Medicine_Donation.ViewModel;
using System.Linq;
using System.Security.Claims;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
    [Area("Admin"), Route("Donor")]
    public class DonorController : BaseController
    {
        private Guid currUserGuid => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userGuid) ? userGuid : Guid.Empty;
        private readonly OnlineMedicineContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public DonorController(OnlineMedicineContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        [Route("DonationRequest")]
        public IActionResult DonationRequest(int? Id)
        {

             var EmergencyRequests = _context.EmergencyRequests.Where(x=>x.Status== "Accepted")
                 .FirstOrDefault(x => x.Id == Id);

             if (EmergencyRequests !=null)
             { 
                 // Create the view model and ensure ALL properties are mapped properly
                 var viewModel = new RequestVM
                 {
                     donationRequest = new DonationRequest()
                     {
                         Id = EmergencyRequests.Id,
                         Name = EmergencyRequests.MedicineName,
                         Quantity = EmergencyRequests.Quantity,
                         Type = EmergencyRequests.Type
                     },
                      NgoList = _context.UserProfiles.Where(u => u.Role == "NGO").ToList()
            };
               
                 return View(viewModel);
             }

             else
            {
                var viewModel = new RequestVM
                {
                    donationRequest = new DonationRequest(),
                    NgoList = _context.UserProfiles.Where(u => u.Role == "NGO").ToList()
                };

                return View(viewModel);
            }
        }

        [Route("RequestMedicine")]
        [HttpPost]
        public async Task<IActionResult> RequestMedicine([FromForm] RequestVM model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            string? filePath = null;

            try
            {
                // Important: Check that we're accessing PhotoFile (from viewmodel), not MedicinePhotoUrl
                if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(model.PhotoFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest("Only image files (.jpg, .jpeg, .png, .gif, .webp) are allowed.");
                    }

                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString("N") + extension;
                    string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.PhotoFile.CopyToAsync(stream);
                    }

                    // Set the path for database storage
                    filePath = "/uploads/" + uniqueFileName;
                }

                var data = new DonationRequest()
                {
                    DonationId = currUserGuid,
                    Name = model.donationRequest.Name,
                    Quantity = model.donationRequest.Quantity,
                    Company = model.donationRequest.Company,
                    Type = model.donationRequest.Type,
                    ExpiryDate = model.donationRequest.ExpiryDate,
                    Condition = model.donationRequest.Condition,
                    SelectNgo = model.donationRequest.SelectNgo,
                    DonationTime = model.donationRequest.DonationTime,
                    Status = model.donationRequest.Status,
                    MedicinePhotoUrl = filePath  // Save the path to database
                };

                _context.DonationRequests.Add(data);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [Route("GetEmergencyRequest")]
        public IActionResult GetEmergencyRequest()
        {
            var acceptedRequests = _context.EmergencyRequests
                .Where(e => e.Status == "Accepted")
                .Select(e => new RequestVM
                {
                    emergencyRequest = e
                })
                .ToList();

            var viewModel = new CombinedRequestVM
            {
                EmergencyRequests = acceptedRequests
            };

            return View(viewModel);
        }

        [Route("DonorHistory")]
        public IActionResult DonorHistory()
        {
            var donationrequest = _context.DonationRequests.Where(x=>x.DonationId == currUserGuid).Select(m => new RequestVM
            {
                donationRequest = new DonationRequest
                {
                    DonationId = m.DonationId, 
                    Name = m.Name,
                    Quantity = m.Quantity,
                    Company = m.Company,
                    Type = m.Type,
                    Condition = m.Condition,
                    ExpiryDate = m.ExpiryDate,
                    SelectNgo = m.SelectNgo,
                    DonationTime = m.DonationTime,
                    Status = m.Status
                }
            }).ToList();
            var viewModel = new CombinedRequestVM
            {
                DonationRequests = donationrequest,
            };

            return View(viewModel);
          
        }

        [HttpPost]
        [Route("AcceptRequest")]
        public IActionResult AcceptRequest(int Id)
        {
            var donationrequest = _context.DonationRequests.Where(x => x.Id == Id).FirstOrDefault();

          
            if (donationrequest != null)
            {
                donationrequest.Status = "Accepted";
                _context.SaveChanges();
            }
            return RedirectToAction("Tables", "Admin");
        }

        [Route("RejectRequest")]
        [HttpPost]
        public IActionResult RejectRequest(int Id)
        {
            var donationrequest = _context.DonationRequests.Where(x => x.Id == Id).FirstOrDefault();

            if (donationrequest != null)
            {
                donationrequest.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Tables", "Admin");
        }

    }
}