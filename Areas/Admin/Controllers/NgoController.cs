using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Medicine_Donation.Controllers;
using Online_Medicine_Donation.Data;
using Online_Medicine_Donation.DataModel;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;
using Online_Medicine_Donation.ViewModel;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
    [Authorize(Roles = "NGO")]
    [Area("Admin"), Route("Ngo")]
    public class NgoController : BaseController
    {
        private Guid currUserGuid => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userGuid) ? userGuid : Guid.Empty;

        public IEnumerable<MedicineInventory> MedicineInventorys { get; private set; }

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
           
                var user = _context.UserProfiles.FirstOrDefault(x => x.UserId == currUserGuid);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var data = new RequestVM()
                {
                    emergencyRequest = new EmergencyRequest()
                    {
                        NgoName = user.FullName
                    }
                };

                return View(data); // Important: Pass model to view
            

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
            var datalist = new List<MedicineInventoryVM>();

            var user = _context.UserProfiles.Where(x => x.UserId == currUserGuid).FirstOrDefault();

            var request = _context.DonationRequests.Where(d => d.Status == "Accepted" && d.SelectNgo== user.FullName).ToList();
            if (request != null)
            {
                foreach (var item in request)
                {
                    bool alreadyExists = _context.MedicineInventories.Any(m => m.DonationRequestsId == item.Id); 

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
                            SelectNgo = @item.SelectNgo,
                            Status = @item.Status,
                            MedicinePhotoUrl = @item.MedicinePhotoUrl
                        };

                        _context.MedicineInventories.Add(inventoryItem);
                        _context.SaveChanges();
                    }

                    var donarinfo = _context.UserProfiles.Where(x => x.UserId == item.DonationId).FirstOrDefault();

                    int quantity = (int) _context.MedicineInventories.Where(x => x.DonationRequestsId == item.Id).Select(x => x.Quantity).FirstOrDefault();

                    var tempdata = new MedicineInventoryVM
                    {
                        Name = item.Name,
                        Quantity = quantity,
                        Type = item.Type,
                        FullName = donarinfo?.FullName,
                        Address = donarinfo?.Address,
                        PhoneNumber = donarinfo?.PhoneNumber,
                        DonationTime = item.DonationTime,
                        SelectNgo = item.SelectNgo
                    };

                    datalist.Add(tempdata);
                    
                }
            }

            return View(datalist);
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
            if (model == null || model.Quantity == null || model.Quantity <= 0)
            {
                return BadRequest("Invalid withdrawal request.");
            }

            var user = currUserGuid;

            // Step 1: Find the inventory item
            var inventoryItem = _context.MedicineInventories
                .FirstOrDefault(m => m.Name == model.MedicineName && m.SelectNgo == model.NgoName);

            if (inventoryItem == null)
            {
                return NotFound("Medicine not found in inventory.");
            }

            // Step 2: Check if enough stock exists
            if (inventoryItem.Quantity < model.Quantity)
            {
                return BadRequest("Insufficient stock.");
            }

            // Step 3: Deduct stock quantity
            inventoryItem.Quantity -= model.Quantity.Value;

            // Step 4: Save or update withdraw request
            var existingRequest = _context.WithdrawRequests
                .FirstOrDefault(x => x.NgoId == user && x.MedicineName == model.MedicineName);

            if (existingRequest == null)
            {
                var withdraw = new WithdrawRequest
                {
                    NgoId = user,
                    NgoName = model.NgoName,
                    MedicineName = model.MedicineName,
                    Quantity = model.Quantity,
                    WithdrawTime = model.WithdrawTime,
                    Purpose = model.Purpose
                };

                _context.WithdrawRequests.Add(withdraw);
            }
            else
            {
                existingRequest.Quantity += model.Quantity;
                existingRequest.WithdrawTime = model.WithdrawTime;
                existingRequest.Purpose = model.Purpose;

                _context.WithdrawRequests.Update(existingRequest);
            }

            // Step 5: Save inventory changes
            _context.MedicineInventories.Update(inventoryItem);
            _context.SaveChanges();

            return RedirectToAction("MedicineStock");
        }


    }
}

