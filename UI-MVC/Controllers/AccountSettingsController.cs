using Microsoft.AspNetCore.Mvc;
using COI.UI.MVC.Models;

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