using Email.Microservice.Core.Interfaces;
using Email.Microservice.Model;
using System.Net;
using System.Net.Mail;

namespace Email.Microservice.Application.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EmailModel email)
        {
            MailMessage mailMessage = new MailMessage("FromEmail@email.com", email.EmailAddress, email.Subject, email.Body);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("FromEmail@email.com", "password");
            smtpClient.Send(mailMessage);
        }
    }
}
