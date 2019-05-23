using COI.UI.MVC.Models;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	[Route("{orgId}/[controller]")]
    public class ContactController : Controller
    {
	    private readonly IEmailService _emailService;

	    public ContactController(IEmailService emailService)
	    {
		    _emailService = emailService;
	    }

	    [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendContact([FromRoute] string orgId, ContactFormModel userInput)
        {
	        _emailService.SendEmail(
		        userInput.Name, userInput.Email,
		        "City of Ideas", "cityofideas@kristofbuts.be",
		        "Contact Form", userInput.Message
		        );
	        
	        return RedirectToAction("Index", "Contact", new {orgId});
        }
    }
}