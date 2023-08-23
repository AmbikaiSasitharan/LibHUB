using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDetailsDTO>> GetAllGenres();
        Task<GenreDetailsDTO> GetGenre(int Id);
        Task<GenreDetailsDTO> AddGenre(GenreToAddDTO genreToAdd);
        Task<GenreDetailsDTO> RemoveGenre(int Id);
    }
}
