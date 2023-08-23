using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDetailsDTO>> GetAllAuthors();
        Task<AuthorDetailsDTO> GetAuthor(int Id);
        Task<AuthorDetailsDTO> AddAuthor(AuthorToAddDTO authorToAdd);
        Task<AuthorDetailsDTO> RemoveAuthor(int Id);
    }
}
