using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IRatingService
    {
        Task<RatingDetailsDTO> GetRating(int Id);
        Task<IEnumerable<RatingDetailsDTO>> GetRatingForBookDescription(int descriptionId);
        Task<RatingDetailsDTO> AddRating(RatingToAddDTO ratingToAddDTO);
        Task<RatingDetailsDTO> RemoveRating(int Id);
        Task<IEnumerable<RatingDetailsDTO>> GetAllRatingsOofAUser(int userId);
    }
}
