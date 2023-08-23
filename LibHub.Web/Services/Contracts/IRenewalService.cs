using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IRenewalService
    {
        Task<RenewalDetailsDTO> GetRenewal(int Id);
        Task<RenewalDetailsDTO> AddRenewal(int borrowId);
        Task<RenewalDetailsDTO> RemoveRenewal(int Id);
        Task<IEnumerable<LogEntryDTO>> GetLogHistory(int userId);
    }
}
