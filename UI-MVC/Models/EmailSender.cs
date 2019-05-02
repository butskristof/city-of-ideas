using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace COI.UI.MVC.Models
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _config;

		public EmailSender(IConfiguration config)
		{
			_config = config;
		}

		public Task SendEmailAsync(string email, string subject, string message)
		{
			return Execute(_config["SendGrid:Key"], subject, message, email);
		}

		public Task Execute(string apiKey, string subject, string message, string email)
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("cityofideas@kristofbuts.be", "City of Ideas"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
			
			// disable click tracking
			msg.SetClickTracking(false, false);

			return client.SendEmailAsync(msg);
		}
	}
}