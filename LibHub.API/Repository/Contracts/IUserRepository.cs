using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> AddUser(UserToAddDTO userToAddDTO);
        Task<User> RemoveUser(int Id);
        Task<User> AddOneToNumBorrowingBooks(int id);
        Task<User> SubtractOneFromNumBorrowingBooks(int id);
        Task<User> UpdateUserInformation(int id, InfoToUpdateUserDTO infoToUpdateUserDTO);
        Task<User> DeactivateUser(int Id);
        Task<User> ActivateUser(int Id);
        Task<IEnumerable<User>> GetUsersWithLateBorrows();
        Task<IEnumerable<User>> GetAllUsersForLibraryStats();


    }
}
