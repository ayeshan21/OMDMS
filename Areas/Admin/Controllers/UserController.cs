using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Medicine_Donation.Controllers;
using Online_Medicine_Donation.Data;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;
using Online_Medicine_Donation.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
    [Area("Admin"), Route("User")]

    public class UserController : BaseController
    {
        private Guid currUserGuid => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userGuid) ? userGuid : Guid.Empty;
        private readonly OnlineMedicineContext _context;
        private readonly IWebHostEnvironment _env;

        private readonly UserManager<IdentityUser> _userManager;

        public UserController(OnlineMedicineContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        [Route("UserProfileindex")]
        public IActionResult UserProfileindex()
        {

            var user = _context.UserProfiles.FirstOrDefault(x => x.UserId == currUserGuid);
    
            if(user == null)
            {
                return View();
            }
            else
            {
              
                return RedirectToAction("UserProfile", user);

            }

        }
        [Route("UserProfile")]

        public IActionResult UserProfile(UserProfile model)
        {

            return View(model);

        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UserProfileVM model)
        {
            if (model == null)
            {
                return BadRequest("Invalid user data.");
            }

            string? filePath = null;

            try
            {
                // Handle profile picture file upload
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(model.ProfilePictureFile.FileName).ToLowerInvariant();

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
                        await model.ProfilePictureFile.CopyToAsync(stream);
                    }

                    filePath = "/uploads/" + uniqueFileName;
                    model.ProfilePictureUrl = filePath;
                }

                var usercheck = _context.UserProfiles.FirstOrDefault(x => x.UserId == model.UserId);

                if (usercheck == null)
                {
                    // Map to UserProfile model
                    var data = new UserProfile()
                    {
                        UserId = currUserGuid,
                        FullName = model.FullName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        NgoCode = model.NgoCode,
                        Role = model.Role,
                        ProfilePictureUrl = model.ProfilePictureUrl
                    };

                    _context.UserProfiles.Add(data);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true });
                }
                else
                {
                    //usercheck profile update
                    usercheck.FullName = model.FullName;
                    usercheck.Email = model.Email;
                    usercheck.PhoneNumber = model.PhoneNumber;
                    usercheck.Address = model.Address;
                    usercheck.NgoCode = model.NgoCode;
                    usercheck.Role = model.Role;
                    usercheck.ProfilePictureUrl = model.ProfilePictureUrl;

                    _context.UserProfiles.Update(usercheck);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error here
                return StatusCode(500, "An error occurred while saving the user profile: " + ex.Message);
            }

        }

        [Route("EditUser")]
        [HttpGet]
        public IActionResult EditUser(Guid id)
        {
            var profile = _context.UserProfiles.FirstOrDefault(x => x.UserId == id);
            if (profile == null)
            {
                return NotFound();
            }

            var model = new UserProfileVM
            {
                UserId = profile.UserId, // ✅ this was missing
                FullName = profile.FullName,
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                NgoCode = profile.NgoCode,
                Role = profile.Role,
                ProfilePictureUrl = profile.ProfilePictureUrl
            };

            return View(model);
        }



        [Route("EditUser")]
        [HttpPost]
        public async Task<IActionResult> EditUser(UserProfileVM model)
        {
            var profile = _context.UserProfiles.FirstOrDefault(x => x.UserId == model.UserId);
            if (profile == null)
            {
                return NotFound();
            }

            // Handle profile image upload (as in your original logic)
            // Update fields
            profile.FullName = model.FullName;
            profile.Email = model.Email;
            profile.PhoneNumber = model.PhoneNumber;
            profile.Address = model.Address;
            profile.NgoCode = model.NgoCode;
            profile.Role = model.Role;
            profile.ProfilePictureUrl = model.ProfilePictureUrl;

            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile");
        }

    }
}
