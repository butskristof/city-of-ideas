using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}