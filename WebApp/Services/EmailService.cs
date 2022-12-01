using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApp.Settings;

namespace WebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SMTPSettings> smtpSettings;

        public EmailService(IOptions<SMTPSettings> smtpSettings)
        {
            this.smtpSettings = smtpSettings;
        }

        public async Task sendAsync(string fromEmail, string ToEmail, string subject, string body)
        {

            var message = new MailMessage(fromEmail, ToEmail, subject, body);

            using (var emailClient = new SmtpClient(smtpSettings.Value.Host, smtpSettings.Value.Port))
            {

                emailClient.Credentials = new NetworkCredential
                    (smtpSettings.Value.UserName, smtpSettings.Value.Password);
                await emailClient.SendMailAsync(message);
            }

        }
    }


}
