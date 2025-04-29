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
    [Area("Admin"),Route("Donor")]
    public class DonorController : BaseController
    {
        private Guid currMedicineGuid => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userGuid) ? userGuid : Guid.Empty;
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
        public IActionResult DonationRequest()
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

            string? filePath = null;

            try
            {
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

                    filePath = "/uploads/" + uniqueFileName;
                }

                var data = new Medicine()
                {
                    MedicineId = currMedicineGuid,
                    Name = model.Name,
                    Quantity = model.Quantity,
                    Company = model.Company,
                    Type = model.Type,
                    ExpiryDate = model.ExpiryDate,
                    Condition = model.Condition,
                    MedicinePhotoUrl = filePath 
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
