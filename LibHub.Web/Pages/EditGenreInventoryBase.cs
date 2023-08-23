using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class EditGenreInventoryBase : ComponentBase
    {
        [Inject]
        public IGenreService GenreService { get; set; }

        public IEnumerable<GenreDetailsDTO> allGenres { get; set; }

        public GenreToAddDTO genreToAdd = new GenreToAddDTO();

        public List<int> SelectedIds = new List<int>();

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                allGenres = await GenreService.GetAllGenres();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddGenre_Click(GenreToAddDTO updatedgenreToAddDTO)
        {
            try
            {
                var genreDetailsDTO = await GenreService.AddGenre(updatedgenreToAddDTO);

                allGenres = await GenreService.GetAllGenres();

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
                    var genreDetailDTO = await GenreService.RemoveGenre(Id);
                }

                allGenres = await GenreService.GetAllGenres();
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
