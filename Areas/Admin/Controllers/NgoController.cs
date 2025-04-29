using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Medicine_Donation.Data;
using Online_Medicine_Donation.DataModel;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;
using Online_Medicine_Donation.ViewModel;
using System.Linq;
using System.Security.Claims;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{

    [Area("Admin"), Route("Ngo")]
    public class NgoController : Controller
    {
        private Guid currMedicineGuid => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userGuid) ? userGuid : Guid.Empty;
        private readonly OnlineMedicineContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NgoController(OnlineMedicineContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("EmergencyRequest")]
        public IActionResult EmergencyRequest()
        {
            return View();
        }

        [Route("CreateMedicine")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromForm] MedicineVM model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var data = new Medicine()
                {
                    MedicineId = currMedicineGuid,
                    Name = model.Name,
                    Quantity = model.Quantity,   
                    Type = model.Type,
                };

                _context.Medicines.Add(data);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
