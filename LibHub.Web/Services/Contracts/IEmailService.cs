using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
