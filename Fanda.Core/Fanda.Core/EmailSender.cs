using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Fanda.Core
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task SendGridEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly AppSettings _appSettings;

        public EmailSender(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_appSettings.MailSettings.Mail, _appSettings.MailSettings.DisplayName);
            message.To.Add(new MailAddress(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;

            message.IsBodyHtml = false;
            message.Body = mailRequest.Body;
            smtp.Port = _appSettings.MailSettings.Port;
            smtp.Host = _appSettings.MailSettings.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_appSettings.MailSettings.Mail, _appSettings.MailSettings.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }

        public Task SendGridEmailAsync(string email, string subject, string message)
            => Execute(_appSettings.FandaSettings.SendGridKey, subject, message, email);

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("fanda@fanda.com", "Fanda"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
