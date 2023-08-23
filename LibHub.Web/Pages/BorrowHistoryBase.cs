using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class BorrowHistoryBase : ComponentBase
    {
        [Parameter]
        public int UserId { get; set; }

                
        [Inject]
        public IRenewalService RenewalService { get; set; }

        public IEnumerable<LogEntryDTO> LogEntryHistory { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsOpened_ViewLogHistory = true;

        public void closeModal_ViewLogHistory()
        {
            IsOpened_ViewLogHistory = false;
        }

        public void openModal_ViewLogHistory()
        {
            IsOpened_ViewLogHistory = true;
        }



        protected override async Task OnInitializedAsync()
        {
            try
            {
                LogEntryHistory = await RenewalService.GetLogHistory(UserId);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
    }
}

