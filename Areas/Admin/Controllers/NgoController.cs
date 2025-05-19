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

    [Area("Admin"), Route("Ngo")]
    public class NgoController : BaseController
    {
        private Guid currUserGuid => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userGuid) ? userGuid : Guid.Empty;
        private readonly OnlineMedicineContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public NgoController(OnlineMedicineContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
            _context = context;
            
            _userManager = userManager;
        }
        [Route("EmergencyRequest")]
        public IActionResult EmergencyRequest()
        {
            return View();
        }

        [Route("RequestMedicine")]
        [HttpPost]
        public async Task<IActionResult> RequestMedicine([FromForm] RequestVM model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var data = new EmergencyRequest()
                {
                    EmergencyId = currUserGuid,
                    NgoName = model.emergencyRequest.NgoName,
                    MedicineName = model.emergencyRequest.MedicineName,
                    Quantity = model.emergencyRequest.Quantity,
                    Type = model.emergencyRequest.Type,
                    Reason = model.emergencyRequest.Reason,
                    Days = model.emergencyRequest.Days
                };

                _context.EmergencyRequests.Add(data);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [Route("NGOHistory")]
        public IActionResult NGOHistory()
        {
            var emergencyrequest = _context.EmergencyRequests.Where(x => x.EmergencyId == currUserGuid).Select(m => new RequestVM
            {
                emergencyRequest = new EmergencyRequest
                {
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
                EmergencyRequests = emergencyrequest,
            };

            return View(viewModel);

        }

        [Route("MedicineStock")]
        public IActionResult MedicineStock()
        {
            var request = _context.DonationRequests.Where(d => d.Status == "Accepted").ToList();

            if (request != null)
            { 
                foreach( var item in request)
                {

                    // Check if already exists to avoid duplicates
                    bool alreadyExists = _context.MedicineInventories
                        .Any(m => m.DonationRequestsId == item.Id); // Or match on name/expiry/etc if needed

                    if (!alreadyExists)
                    {
                        var inventoryItem = new MedicineInventory
                        {
                            UserId = @item.DonationId,
                            DonationRequestsId = (int)@item.Id,
                            Name = @item.Name,
                            Quantity = @item.Quantity,
                            Type = @item.Type,
                            Company = @item.Company,
                            ExpiryDate = @item.ExpiryDate,
                            Condition = @item.Condition,
                            Status = @item.Status,
                            MedicinePhotoUrl = @item.MedicinePhotoUrl
                        };

                        _context.MedicineInventories.Add(inventoryItem);
                        _context.SaveChanges();
                    }
                }
            }

            // Now safely create the view model regardless
            var DonationRequests = _context.DonationRequests
                .Where(x => x.Status == "Accepted")
                .ToList();

            var viewModel = new CombinedRequestVM
            {
                DonationRequests = DonationRequests.Select(d => new RequestVM
                {
                    donationRequest = d,
                    userProfile = _context.UserProfiles.FirstOrDefault(u => u.UserId == d.DonationId)
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("ReceiveRequest")]
        public IActionResult ReceiveRequest(int DonationId)
        {
            var request = _context.MedicineInventories.Where(x => x.Id == DonationId).FirstOrDefault();

            if (request != null && request.Status == "Accepted")
            {
                request.Status = "Received";
                _context.SaveChanges();
            }

            return RedirectToAction("MedicineStock");
        }


        [HttpPost]
        [Route("Withdraw")]
        public IActionResult Withdraw([FromForm] WithdrawRequestVM model)
        {
            if (model == null)
            {
                return NotFound();
            }
            var withdraw = new WithdrawRequest
            {
                NgoId = currUserGuid,
                NgoName = model.NgoName,
                MedicineName = model.MedicineName,
                Quantity = model.Quantity,
                WithdrawTime = model.WithdrawTime,
                Purpose = model.Purpose,
            };
            _context.WithdrawRequests.Add(withdraw);
            _context.SaveChanges();
            return RedirectToAction("MedicineStock");
        }
    }
}

