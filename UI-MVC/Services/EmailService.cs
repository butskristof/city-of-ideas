using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace COI.UI.MVC.Services
{
	public interface IEmailService
	{
		Task SendEmail(string fromName, string fromEmail, string toName, string toEmail,
			string subject, string message);
	}

	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task SendEmail(string fromName, string fromEmail, string toName, string toEmail,
			string subject, string message)
		{
			var apiKey = _configuration["SendGrid:Key"];
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(fromEmail, fromName),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(toEmail, toName));
			
			// disable click tracking
			msg.SetClickTracking(false, false);

			return client.SendEmailAsync(msg);
		}
	}
}