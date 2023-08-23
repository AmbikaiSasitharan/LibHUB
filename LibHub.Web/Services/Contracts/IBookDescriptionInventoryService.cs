using LibHub.Models.DTOs;

namespace LibHub.Web.Services.Contracts
{
    public interface IBookDescriptionInventoryService
    {
        Task<IEnumerable<BookDescriptionInventoryDTO>> GetBookDescriptions();
        Task<BookDescriptionDetailsDTO> GetBookDescription(int id);
        Task<BookDescriptionDetailsDTO> RemovingAuthorFromBookDescription(AuthorAndBookDescriptionRelationshipDTO authorAndBookDescriptionRelationshipDTO);
        Task<BookDescriptionDetailsDTO> AddingAuthorToBookDescription(AuthorAndBookDescriptionRelationshipDTO authorAndBookDescriptionRelationshipDTO);
        Task<BookDescriptionDetailsDTO> AddBookDescription(BookDescriptionToAddDTO bookDescriptionToAddDTO);
        Task<BookDescriptionDetailsDTO> RemoveBookDescription(int Id);
        Task<BookDescriptionDetailsDTO> DeactivateBookDescription(int Id);
        Task<BookDescriptionDetailsDTO> ActivateBookDescription(int Id);

    }
}
