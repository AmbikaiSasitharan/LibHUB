using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IBookDescriptionRepository
    {
        Task<IEnumerable<BookDescription>> GetBookDescriptions();
        Task <BookDescription> GetBookDescription(int id);
        Task<BookDescription> SubtractOneFromNumAvailable (int id);
        Task<BookDescription> AddOneToNumAvailable(int id);
        Task<BookDescription> SubtractOneFromNumCopies(int id);
        Task<BookDescription> AddOneToNumCopies(int id);
        Task<BookDescription> RemoveAuthorFromBookDescription(int Id, int authorId);
        Task<BookDescription> AddAuthorToBookDescription(int Id, int authorId);
        Task<BookDescription> AddGenreToBookDescription(int Id, int genreId);
        Task<BookDescription> AddBookToBookDescription(int Id, int bookId);
        Task<BookDescription> AddRatingToBookDescription(int Id, float ratingToAdd);
        Task<BookDescription> RemoveRatingFromBookDescription(int Id, float ratingToRemove);
        Task<BookDescription> RemoveBookFromBookDescription(int  Id, int bookId);
        Task<Genre> RemoveGenreFromBookDescriptions(List<BookDescription> bookDescriptions, int genreId);
        Task<Author> RemoveAuthorFromBookDescriptions(List<BookDescription> bookDescriptions, int authorId);
        Task<BookDescription> AddBookDescription(BookDescriptionToAddDTO bookDescriptionToAddDTO);
        Task<BookDescription> RemoveBookDescription(int Id);
        Task<BookDescription> DeactivateBookDescription(int Id);
        Task<BookDescription> ActivateBookDescription(int Id);

    }
}
