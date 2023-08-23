using Microsoft.AspNetCore.Components;
using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;

namespace LibHub.Web.Pages
{
    public class UserBookSelectionBase: ComponentBase
    {
        [Parameter]
        public int UserId { get; set; }

        [Inject]
        public IBookDescriptionInventoryService BookDescriptionInventoryService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public IEnumerable<BookDescriptionInventoryDTO> BookDescriptionInventory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BookDescriptionInventory = await BookDescriptionInventoryService.GetBookDescriptions();
            BookDescriptionInventory = BookDescriptionInventory.OrderBy(bd => bd.IsActive ? 0 : 1);
        }

        protected async Task NavigateBackToUser()
        {
            NavigationManager.NavigateTo($"/displayuserdetails/{UserId}");
        }

    }
}
