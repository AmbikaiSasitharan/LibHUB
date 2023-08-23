using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IEmailRepository
    {
        void SendEmail(EmailDTO request);
    }
}
