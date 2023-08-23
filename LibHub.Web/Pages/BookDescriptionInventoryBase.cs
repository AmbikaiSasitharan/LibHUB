using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class BookDescriptionInventoryBase : ComponentBase
    {
        [Inject]
        public IBookDescriptionInventoryService BookDescriptionInventoryService { get; set; }

        public IEnumerable<BookDescriptionInventoryDTO> BookDescriptionInventory { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BookDescriptionInventory = await BookDescriptionInventoryService.GetBookDescriptions();
            BookDescriptionInventory = BookDescriptionInventory.OrderBy(bd => bd.IsActive ? 0 : 1);
        }
        public void NavigateToAddBookDescripionPage()
        {
            NavigationManager.NavigateTo("/addBookDescriptionPage");
        }

    }
}
