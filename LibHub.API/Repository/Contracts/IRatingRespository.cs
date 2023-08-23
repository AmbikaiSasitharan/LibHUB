using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IRatingRespository
    {
        Task<Rating> GetRating(int Id);
        Task<IEnumerable<Rating>> GetRatingsForBookDescription(int bookDescriptionId);
        Task<Rating> AddRating(RatingToAddDTO ratingToAddDTO, User user, BookDescription bookDescription);
        Task<IEnumerable<Rating>> RemoveAllRatingsOfBookDescription(int bookDescriptionId);
        Task<IEnumerable<Rating>> RemovwAllRatingsOfAUser(int userId);
        Task<Rating> RemoveRating (int Id);
        Task<IEnumerable<Rating>> GetAllRatingsOfAUser (int userId);    
    }
}
