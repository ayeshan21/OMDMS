using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Online_Medicine_Donation.Data;
using System;
using System.Linq;
using System.Security.Claims;

namespace Online_Medicine_Donation.Controllers
{
    public class BaseController : Controller
    {
        protected readonly OnlineMedicineContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BaseController(OnlineMedicineContext context, UserManager<IdentityUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedUserId))
            {
                var profile = _context.UserProfiles
                    .FirstOrDefault(x => x.UserId == parsedUserId);

                if (profile != null)
                {
                    ViewBag.ProfilePictureUrl = profile.ProfilePictureUrl;
                }
                else
                {
                    ViewBag.ProfilePictureUrl = null; // or set a default profile picture URL if you prefer
                }
            }
            else
            {
                ViewBag.ProfilePictureUrl = null;
            }
        }
    }
}
