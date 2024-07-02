namespace EduPortal.Application.Interfaces.Services
{
    public interface IMailService
    {
        void SendEmailWithMailKitPackage(string subject, string body, string mail);

    }
}
