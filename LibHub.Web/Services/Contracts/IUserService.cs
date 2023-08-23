using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserInventoryDTO>> GetUsers();
        Task<UserDetailsWIthLateBorrowDetailsDTO> GetUser(int id);
        Task<IEnumerable<UserWithLateBorrowsDTO>> GetUsersWithFeesFined();
        Task<IEnumerable<UserWithLateBorrowsDTO>> GetUsersWithLateBorrowButNoFeesFined();
        Task<UserDetailsDTO> AddUser(UserToAddDTO userToAddDTO);
        Task<UserDetailsDTO> RemoveUser(int Id);
        Task<UserDetailsDTO> UpdateUserInformation(int Id, InfoToUpdateUserDTO infoToUpdateUserDTO);
        Task<UserDetailsDTO> DeactivateUser(int Id);
        Task<UserDetailsDTO> ActivateUser(int Id);
        Task<UserWithLateBorrowsDTO> GetUserWithLateBorrowDetails(int id);
    }
}
