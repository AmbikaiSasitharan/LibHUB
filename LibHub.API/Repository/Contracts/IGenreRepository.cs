using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IGenreRepository
    {
        Task<Genre> AddBookDescriptionToGenre(int Id, int bookDescriptionId);
        Task<Genre> GetGenre(int Id);
        Task<BookDescription> RemoveBookDescriptionFromGenres(List<Genre> genres, int bookDescriptionId);
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<Genre> AddGenre(GenreToAddDTO genreToAddDTO);
        Task<Genre> RemoveGenre(int Id);
    }
}
