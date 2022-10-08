using Email.Microservice.Model;

namespace Email.Microservice.Core.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailModel email);
    }
}
