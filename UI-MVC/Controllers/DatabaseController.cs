using System.Threading.Tasks;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	public class DatabaseController : Controller
	{
		private readonly ISeedService _seedService;

		public DatabaseController(ISeedService seedService)
		{
			_seedService = seedService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Seed()
		{
			await _seedService.Seed();
			return RedirectToAction("Index");
		}
	}
}