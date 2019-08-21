using CSC.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace CSC.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Configuration

        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                return Execute(Options.SendGridKey, subject, message, email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            try
            {
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("wmsolucoes_suporte@hotmail.com", "Wm Soluções"),
                    Subject = subject,
                    PlainTextContent = message + "\nEmail Automático, <strong>Não Responde<strong>",
                    HtmlContent = message
                };
                msg.AddTo(new EmailAddress(email));

                // Disable click tracking.
                // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
                msg.SetClickTracking(false, false);

                return client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}