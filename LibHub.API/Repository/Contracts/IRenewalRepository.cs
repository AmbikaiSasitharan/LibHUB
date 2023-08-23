using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IRenewalRepository
    {
        Task<Renewal> GetRenewal(int Id);
        Task<Renewal> AddRenewal(Borrow borrow);
        Task<Renewal> RemoveRenewal(int Id);
        Task<IEnumerable<Renewal>> GetAllRenewalsOfReturnedBorrows(int userId);
    }
}
