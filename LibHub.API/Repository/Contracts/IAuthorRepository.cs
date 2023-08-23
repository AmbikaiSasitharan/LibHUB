using LibHub.API.Entities;
using LibHub.Models.DTOs;

namespace LibHub.API.Repository.Contracts
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthor(int Id);
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author> AddAuthor(AuthorToAddDTO authorToAdd);
        Task<Author> RemoveBookDescriptionFromAuthor(int Id, int bookDescriptionId);
        Task<Author> AddBookDescriptionToAuthor(int Id, int bookDescriptionId);
        Task<Author> RemoveAuthor(int Id);
        void AddBookDescriptionToAuthorGivenEntities(Author author, BookDescription bookDescription);
        Task<BookDescription> RemoveBookDescriptionFromAuthors(List<Author> authors, int bookDescription);
    }
}
