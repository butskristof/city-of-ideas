using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	[Route("{orgId}/[controller]")]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendContact()
        {
	        return RedirectToAction("Index");
        }
    }
}