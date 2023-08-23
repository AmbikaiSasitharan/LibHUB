using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookDetailsDTO>> GetBooksForBookDescription(int bookDescriptionId);
        Task<BookDetailsDTO> GetBook(int id);
        Task<BookDetailsDTO> AddBook(BookToAddDTO bookToAddDTO);
        Task<BookDetailsDTO> RemoveBook(int Id);
        Task<IEnumerable<BookDetailsDTO>> GetAllBooks();
    }
}
