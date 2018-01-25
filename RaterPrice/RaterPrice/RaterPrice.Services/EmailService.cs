using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace RaterPrice.Services
{
    public class EmailService : IIdentityMessageService
    {

        public EmailService()
        {

        }

        public Task SendAsync(IdentityMessage message)
        {

            var credentialUserName = ConfigurationManager.AppSettings["SmtpUserName"];
            var sentFrom = ConfigurationManager.AppSettings["SmtpFrom"];
            var pwd = ConfigurationManager.AppSettings["SmtpPassword"];

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);

            client.Port =Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            // Send:
            return client.SendMailAsync(mail);
        }
    }
}
