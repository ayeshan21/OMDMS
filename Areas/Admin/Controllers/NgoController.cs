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

        public NgoController(OnlineMedicineContext context, UserManager<IdentityUser> userManager): base(context, userManager)
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
                    Reason = model.emergencyRequest.Reason
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
                    Status = m.Status
                }
            }).ToList();
            var viewModel = new CombinedRequestVM
            {
                EmergencyRequests = emergencyrequest,
            };

            return View(viewModel);

        }
    }
}
