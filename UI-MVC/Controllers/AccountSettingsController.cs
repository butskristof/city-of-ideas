using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    public class AccountSettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}