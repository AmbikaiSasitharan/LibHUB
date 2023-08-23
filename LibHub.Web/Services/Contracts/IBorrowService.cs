using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IBorrowService
    {
        Task<IEnumerable<BorrowDetailsDTO>> GetBorrowHistoryOfAUser(int userId1);
        Task<List<BorrowDetailsDTO>> GetCurrentBorrowsOfAUser(int userId2);
        Task<BorrowDetailsDTO> GetBorrow(int id);
        Task<BorrowDetailsDTO> AddBorrow(BorrowToAddDTO borrowToAddDTO);
        Task<BorrowDetailsDTO> RemoveBorrow(int id);
        Task<BorrowDetailsDTO> ReturnBorrow(int id, BorrowDetailsDTO borrowDetailsDTO);
        Task<IEnumerable<BorrowDetailsDTO>> GetAllBorrowsNotReturned();
        void UpdateBorrowIsLateNotified(int borrowId);
        void UpdateBorrowIsFineNotified(int borrowId);
    }
}
