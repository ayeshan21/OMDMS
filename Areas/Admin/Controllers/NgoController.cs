using Microsoft.AspNetCore.Mvc;

namespace Online_Medicine_Donation.Areas.Admin.Controllers
{
    [Area("Admin"), Route("Ngo")]
    public class NgoController : Controller
    {
          public IActionResult NgoDashboard()
        {
            return View();
        }
    }
}
