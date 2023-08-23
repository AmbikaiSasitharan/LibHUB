using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class DisplayBookDescriptionsBase :ComponentBase

    {
        [Parameter]
        public IEnumerable<BookDescriptionInventoryDTO> bookDescriptionInventoryDTO { get; set; }

        [Inject]
        public IBookDescriptionInventoryService bookDescriptionInventoryService { get; set; }
        
        public List<int> SelectedIds = new List<int>();

        protected async void DeactivateSelectedValues()
        {
            try
            {
                foreach (int Id in SelectedIds)
                {
                    var bookDescriptionDetailDTO = await bookDescriptionInventoryService.DeactivateBookDescription(Id);
                }

                bookDescriptionInventoryDTO = await bookDescriptionInventoryService.GetBookDescriptions();
                bookDescriptionInventoryDTO = bookDescriptionInventoryDTO.OrderBy(bd => bd.IsActive ? 0 : 1);
                StateHasChanged();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async void ActivateSelectedValues()
        {
            try
            {
                foreach (int Id in SelectedIds)
                {
                    var bookDescriptionDetailDTO = await bookDescriptionInventoryService.ActivateBookDescription(Id);
                }

                bookDescriptionInventoryDTO = await bookDescriptionInventoryService.GetBookDescriptions();
                bookDescriptionInventoryDTO = bookDescriptionInventoryDTO.OrderBy(bd => bd.IsActive ? 0 : 1);
                StateHasChanged();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void CheckboxClicked(int aSelectedId, object aChecked)
        {
            if ((bool)aChecked)
            {
                if (!SelectedIds.Contains(aSelectedId))
                {
                    SelectedIds.Add(aSelectedId);
                }
            }
            else
            {
                if (SelectedIds.Contains(aSelectedId))
                {
                    SelectedIds.Remove(aSelectedId);
                }
            }
            StateHasChanged();
        }
    }
}
