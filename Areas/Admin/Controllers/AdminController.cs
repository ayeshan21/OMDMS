using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Medicine_Donation.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
    [Area("Admin"), Route("Admin")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly OnlineMedicineContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(OnlineMedicineContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            //var userId = UserManager.GetUserId(User);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var profile = _context.UserProfiles.Where(x=>x.UserProfileId.ToString() == userId).FirstOrDefault();

            ViewBag.ProfilePictureUrl = profile?.ProfilePictureUrl;

            return View(profile);
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
            var profileUserIds = _context.UserProfiles
                .Select(x => x.UserId)
                .ToHashSet();

            var allUsers = _context.Users
                .ToList();

            var datalist = allUsers
                .Where(user =>
                    Guid.TryParse(user.Id, out var parsedId) &&
                    !profileUserIds.Contains(parsedId))
                .ToList();

            return View(datalist);
        }
    }
}
