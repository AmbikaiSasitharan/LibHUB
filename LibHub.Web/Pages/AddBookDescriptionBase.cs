using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.LinearGauge.Internal;
using Syncfusion.Blazor.PivotView;
using Syncfusion.Blazor.RichTextEditor;
using System.Reflection.Metadata.Ecma335;

namespace LibHub.Web.Pages
{
    public class AddBookDescriptionBase : ComponentBase
    {
        [Inject]
        public IBookDescriptionInventoryService bookDescriptionService { get; set; }

        [Inject]
        public IAuthorService authorService { get; set; }
        public IEnumerable<AuthorDetailsDTO> allAuthors { get; set; }
        public List<int> selectedAuthors { get; set; }

        [Inject]
        public IGenreService genreService { get; set; }
        public IEnumerable<GenreDetailsDTO> allGenres { get; set; }
        public List<int> selectedGenres { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public BookDescriptionToAddDTO bookDescriptionToAdd = new BookDescriptionToAddDTO();
        public string ErrorMessage { get; set; }

        public bool IsVisible = false;

        public bool IsVisible_ForGenre = false;

        public bool IsVisible_ToAddBookDescriptionConfirmation = false;

        public bool IsOpened_ForEditAuthorInventory = false;

        public async void closeModal_ForEditAuthorInventory()
        {
            allAuthors = await authorService.GetAllAuthors();
            IsOpened_ForEditAuthorInventory = false;
        }

        public void openModal_ForEditAuthorInventory()
        {
            IsOpened_ForEditAuthorInventory = true;
        }

        public bool IsOpened_ForEditGenreInventory = false;

        public async void closeModal_ForEditGenreInventory()
        {
            allGenres = await genreService.GetAllGenres();
            IsOpened_ForEditGenreInventory = false;
        }

        public void openModal_ForEditGenreInventory()
        {
            IsOpened_ForEditGenreInventory = true;
        }

        public bool IsOpened_ForAddBookDescription = false;

        public void cancelModal_ForAddBookDescription()
        {
            IsOpened_ForAddBookDescription = false;
        }

        public async void confirmModal_ForAddBookDescription()
        {
            IsOpened_ForAddBookDescription = false;
            try
            {
                var bookDescriptionDetailsDTO = await bookDescriptionService.AddBookDescription(bookDescriptionToAdd);

                NavigationManager.NavigateTo("/BooksInventory");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void openModal_ForAddBookDescription()
        {
            IsOpened_ForAddBookDescription = true;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {

                bookDescriptionToAdd.AuthorIds = new List<int>(); 
                bookDescriptionToAdd.GenreIds = new List<int>(); 
                                
                allAuthors = new List<AuthorDetailsDTO>();
                allGenres = new List<GenreDetailsDTO>();

                allAuthors = await authorService.GetAllAuthors();
                allGenres = await genreService.GetAllGenres();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected void AddBookDescription_Click()
        {
            
            IsOpened_ForAddBookDescription = true;
        }

        protected async Task OnDialogButtonClick_ToConfirmAddBookDescription()
        {
            try
            {
                var bookDescriptionDetailsDTO = await bookDescriptionService.AddBookDescription(bookDescriptionToAdd);

                NavigationManager.NavigateTo("/BooksInventory");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void OnDialogButtonClick_ToCancelAddBookDescription()
        {
            IsVisible_ToAddBookDescriptionConfirmation = false;
        }
    }
}
