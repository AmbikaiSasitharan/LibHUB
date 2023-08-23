using LibHub.API.Entities;
using LibHub.Models.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LibHub.API.Repository.Contracts
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<Borrow>> GetCurrentBorrowsOfAUser(int UserId);
        Task<IEnumerable<Borrow>> GetBorrowHistoryOfAUser(int UserId);
        Task<IEnumerable<Borrow>> GetLateBorrowOfAUser(int UserId);
        Task<Borrow> GetBorrow(int Id);
        Task<Borrow> AddBorrow(BorrowToAddDTO borrowToAddDTO, Book book, User user);
        Task<Borrow> RemoveBorrow(int Id);
        Task<IEnumerable<Borrow>> RemovwAllBorrowsOfAUser(int userId);
        Task<Borrow> ReturnBorrow(int Id);
        Task<Borrow> AddRenewalToBorrow(int Id, int renewalId);
        Task<Borrow> RemoveRenewalFromBorrow(int Id, int renewalId);
        Task<Borrow> AddOneToNumRenewals(int Id);
        Task<Borrow> Add14DaysToDueDate(int Id);
        Task<Borrow> SubtractOneFromNumRenewals(int Id);
        Task<IEnumerable<Borrow>> GetAllReturnedBorrows(int userId);
        Task<IEnumerable<Borrow>> GetAllBorrowsForLibraryStats();
        Task<Borrow> ChangeIsLateNotified(int borrowId);
        Task<Borrow> ChangeIsFineNotified(int borrowId);
    }
}
