using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LILI_VMS.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {

            var smtpClient = new SmtpClient(mailSettings.SMTP_SERVER)
            {
                Port = mailSettings.PORT,

            };
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("Electronic Mail <ear@aci-bd.com>"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                // Send the email asynchronously
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}
