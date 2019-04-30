using System.Text.Encodings.Web;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using COI.BL.User;
using COI.UI.MVC.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace COI.UI.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterUser> _logger;
//        private readonly IEmailSender _emailSender;
        
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ILogger<RegisterUser> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
//            _emailSender = emailSender;
        }
        
        // GET
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
//            var returnUrl = returnUrl ?? Url.Content("~/");
            var returnUrl = "/";
            if (ModelState.IsValid)
            {
                var user = new User { LastName = registerUser.LastName, FirstName = registerUser.FirstName, UserName = registerUser.Email, Email = registerUser.Email };
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                    var callbackUrl = Url.Page(
//                        "/Account/ConfirmEmail",
//                        pageHandler: null,
//                        values: new { Id = user.Id, code = code },
//                        protocol: Request.Scheme);

//                    await _emailSender.SendEmailAsync(registerUser.Email, "Confirm your email",
//                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }
    }
}