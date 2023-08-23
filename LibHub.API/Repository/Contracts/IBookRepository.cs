using LibHub.API.Entities;
using LibHub.Models.DTOs;
using System.Collections.Generic;

namespace LibHub.API.Repository.Contracts
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksForBookDescription(int bookDescriptionId);
        Task <Book> GetBook(int id);
        Task<Book> ChangeStatusToUnavailable(int id);
        Task<Book> ChangeStatusToAvailable(int id);
        Task<Book> AddBook(BookToAddDTO bookToAddDTO, BookDescription bookDescription);
        Task<Book> RemoveBook(int Id);
        Task<IEnumerable<Book>> GetAllBooksForLibraryStats();
    }
}
