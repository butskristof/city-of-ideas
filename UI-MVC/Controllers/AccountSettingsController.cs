using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	[Route("{orgId}/[controller]")]
    public class AccountSettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}