using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.LinearGauge.Internal;
using System.Reflection.Metadata.Ecma335;

namespace LibHub.Web.Pages
{
    public class AddUserBase:ComponentBase
    {
        [Inject]
        IUserService userService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public UserToAddDTO userToAdd = new UserToAddDTO();
        
        public bool IsVisible_ToAddUserConfirmation = false;

        public bool IsOpened_ForAddUser = false;

        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected void AddUser_Click()
        {
            IsVisible_ToAddUserConfirmation = true;
        }

        protected async Task OnDialogButtonClick_ToConfirmAddUser()
        {
                try
                {
                    var userDetailsDTO = await userService.AddUser(userToAdd);

                    NavigationManager.NavigateTo("/UserInventory");
                }
                catch (Exception)
                {
                    throw;
                }
        }

        public void OnDialogButtonClick_ToCancelAddUser()
        {
            IsVisible_ToAddUserConfirmation = false;
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public void openModal_ToAddUser()
        {
            IsOpened_ForAddUser = true;
        }

        public void cancelModal_ToAddUser()
        {
            IsOpened_ForAddUser = false;
        }

        public async void confirmModal_ToAddUser()
        {
            IsOpened_ForAddUser = false;

            try
            {
                var userDetailsDTO = await userService.AddUser(userToAdd);
                NavigationManager.NavigateTo("/UserInventory");
            }
            catch (Exception)
            {

                throw;
            }
        }

        /*protected async Task AddUser_Click()
        {
            try
            {
                var userDetailsDTO = await userService.AddUser(userToAdd);

                NavigationManager.NavigateTo("/UserInventory");
            }
            catch (Exception)
            {

                throw;
            }
        }*/
    }
}
