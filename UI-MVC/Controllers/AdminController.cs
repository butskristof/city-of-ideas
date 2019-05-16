using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    public class AdminController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}