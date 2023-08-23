using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class UserInventoryBase : ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }

        public IEnumerable<UserInventoryDTO> UserInventory { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public List<int> SelectedIds = new List<int>();

        public void NavigateToAddUserPage()
        {
            NavigationManager.NavigateTo("/addUserPage");
        }

        protected override async Task OnInitializedAsync()
        {
            UserInventory = await UserService.GetUsers();
            UserInventory = UserInventory.OrderBy(bd => bd.IsActive ? 0 : 1);
        }

        protected async void DeactivateSelectedValues()
        {
            try
            {
                foreach (int Id in SelectedIds)
                {
                    var userDetailDTO = await UserService.DeactivateUser(Id);
                }

                UserInventory = await UserService.GetUsers();
                UserInventory = UserInventory.OrderBy(bd => bd.IsActive ? 0 : 1);

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
                    var userDetailDTO = await UserService.ActivateUser(Id);
                }

                UserInventory = await UserService.GetUsers();
                UserInventory = UserInventory.OrderBy(bd => bd.IsActive ? 0 : 1);

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
