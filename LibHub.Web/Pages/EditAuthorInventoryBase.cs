using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class EditAuthorInventoryBase : ComponentBase
    {        
        [Inject]
        public IAuthorService AuthorService { get; set; }

        public IEnumerable<AuthorDetailsDTO> allAuthors { get; set; }

        public AuthorToAddDTO authorToAdd = new AuthorToAddDTO();

        public List<int> SelectedIds = new List<int>();

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                allAuthors = await AuthorService.GetAllAuthors();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddAuthor_Click(AuthorToAddDTO updatedAuthorToAddDTO)
        {
            try
            {
                List<string> names = new List<string> { updatedAuthorToAddDTO.FName, updatedAuthorToAddDTO.MName, updatedAuthorToAddDTO.LName };
                var fullName = string.Join(" ", names);
                
                updatedAuthorToAddDTO.FullName = fullName;
                
                var authorDetailsDTO = await AuthorService.AddAuthor(updatedAuthorToAddDTO);
                
                allAuthors = await AuthorService.GetAllAuthors();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async void RemoveSelectedValues()
        {
            try
            {
                foreach (int Id in SelectedIds)
                {
                    var authorDetailDTO = await AuthorService.RemoveAuthor(Id);
                }

                allAuthors = await AuthorService.GetAllAuthors();
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
