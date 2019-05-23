using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace COI.UI.MVC.Services
{
	/// <summary>
	/// Simple implementation of an IEmailSender using SendGrid
	/// Can be used for Identity e-mailverification and password reset.
	/// disabled for development
	/// </summary>
	public class EmailSender : IEmailSender
	{
		private readonly IEmailService _emailService;

		public EmailSender(IEmailService emailService)
		{
			_emailService = emailService;
		}

		public Task SendEmailAsync(string email, string subject, string message)
		{
			return _emailService.SendEmail(
				"City of Ideas", "cityofideas@kristofbuts.be",
				null, email, subject, message
			);
		}
	}
}