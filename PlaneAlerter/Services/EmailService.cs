using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PlaneAlerter.Services {
	internal interface IEmailService
	{
		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="message">Message to send</param>
		/// <param name="emailAddress">Email address to send to</param>
		Task SendEmail(MailMessage message, string emailAddress);

		/// <summary>
		/// Send a short test message using the supplied (possibly unsaved) settings
		/// </summary>
		/// <returns>Null on success, otherwise the error text</returns>
		Task<string?> SendTestEmail(Models.Settings settings, string toAddress);
	}

	/// <summary>
	/// Class for email operations
	/// </summary>
	internal class EmailService : IEmailService
	{
        private readonly ISettingsManagerService _settingsManagerService;
        private readonly ILoggerWithQueue _logger;

		public EmailService(ISettingsManagerService settingsManagerService, ILoggerWithQueue logger)
		{
			_settingsManagerService = settingsManagerService;
            _logger = logger;
		}

		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="message">Message to send</param>
		/// <param name="emailAddress">Email address to send to</param>
		public async Task<string?> SendTestEmail(Models.Settings settings, string toAddress)
		{
			try
			{
				using var message = new MailMessage
				{
					From = new MailAddress(settings.SenderEmail, "PlaneAlerter Alerts"),
					Subject = "PlaneAlerter test email",
					IsBodyHtml = true,
					Body = "<div style='font-family:Segoe UI,Arial,sans-serif;'><h2 style='color:#1B3A5C;margin:0 0 8px;'>PlaneAlerter</h2>" +
					       "<p>This is a test message. Your SMTP settings are working.</p>" +
					       $"<p style='color:#5B6B7B;font-size:12px;'>Sent {DateTime.Now:yyyy-MM-dd HH:mm:ss} via {settings.SMTPHost}:{settings.SMTPPort}</p></div>"
				};
				message.To.Add(toAddress);

				using var mailClient = new SmtpClient(settings.SMTPHost)
				{
					Port = settings.SMTPPort,
					Credentials = new NetworkCredential(settings.SMTPUser, settings.SMTPPassword),
					EnableSsl = settings.SMTPSSL,
					Timeout = 20000
				};

				await mailClient.SendMailAsync(message);
				return null;
			}
			catch (Exception e)
			{
				return e.InnerException != null ? $"{e.Message} ({e.InnerException.Message})" : e.Message;
			}
		}

		public async Task SendEmail(MailMessage message, string emailAddress) {
            using var mailClient = new SmtpClient(_settingsManagerService.Settings.SMTPHost)
            {
	            Port = _settingsManagerService.Settings.SMTPPort,
	            Credentials = new NetworkCredential(_settingsManagerService.Settings.SMTPUser,
		            _settingsManagerService.Settings.SMTPPassword),
	            EnableSsl = _settingsManagerService.Settings.SMTPSSL
            };

            //Set sender email to the one set in settings
			try
			{
				message.From = new MailAddress(_settingsManagerService.Settings.SenderEmail, "PlaneAlerter Alerts");
			}
			catch
			{
				_logger.Log(
					"ERROR: Email to send from is invalid (" + _settingsManagerService.Settings.SenderEmail + ")",
					Color.Red);
				return;
			}

			//Add email to message receiver list
			try
			{
				message.To.Clear();
				message.To.Add(emailAddress);
			}
			catch
			{
				_logger.Log("ERROR: Email to send to is invalid (" + emailAddress + ")", Color.Red);
				return;
			}


			//Send the alert
			try {
				await mailClient.SendMailAsync(message);
			}
			catch (SmtpException e) {
				if (e.InnerException != null) {
					_logger.Log("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}

				_logger.Log("SMTP ERROR: " + e.Message, Color.Red);
			}
			catch (InvalidOperationException e) {
				if (e.InnerException != null) {
					_logger.Log("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}

				_logger.Log("SMTP ERROR: " + e.Message, Color.Red);
			}
		}
	}
}
