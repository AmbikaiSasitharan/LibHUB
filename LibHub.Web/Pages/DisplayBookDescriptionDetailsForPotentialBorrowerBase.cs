using Microsoft.AspNetCore.Components;
using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using LibHub.Web.Services;

namespace LibHub.Web.Pages
{
    public class DisplayBookDescriptionDetailsForPotentialBorrowerBase: ComponentBase
    {
        [Parameter]
        public int bookDescriptionId { get; set; }
        [Parameter]
        public int userId { get; set; }
        [Inject]
        public IBookDescriptionInventoryService BookDescriptionInventoryService { get; set; }

        public BookDescriptionDetailsDTO BookDescriptionDetails { get; set; }

        [Inject]
        public IBookService BookService { get; set; }

        public IEnumerable<BookDetailsDTO> BookDetails { get; set; }

        [Inject]
        public IBorrowService borrowService { get; set; }

        [Inject]
        public IRatingService ratingService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        public string GenresInOneString { get; set; }
        public string TitleToDisplayDurigBorrowConfirmation { get; set; }
        public int IDToDisplayDuringBorrowConfirmation { get; set; }
        public string LanguageToDisplayDuringBorrowConfirmation { get; set; }
        public bool IsOpened_ForAddBorrow = false;


        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public bool IsVisible_ToAddBorrow = false;

        public string ErrorMessage { get; set; }

        public IEnumerable<RatingDetailsDTO> allRatingsForGivenBookDescription { get; set; }

        public UserDetailsWIthLateBorrowDetailsDTO User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                BookDescriptionDetails = await BookDescriptionInventoryService.GetBookDescription(bookDescriptionId);
                BookDetails = await BookService.GetBooksForBookDescription(BookDescriptionDetails.Id);
                GenresInOneString = string.Join(",", BookDescriptionDetails.Genres);
                allRatingsForGivenBookDescription = await ratingService.GetRatingForBookDescription(BookDescriptionDetails.Id);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddBorrow_Click(int bookID)
        {
            var book = await BookService.GetBook(bookID);
            var bookDescription = await BookDescriptionInventoryService.GetBookDescription(book.BookDescriptionId);

            TitleToDisplayDurigBorrowConfirmation = bookDescription.Title;
            IDToDisplayDuringBorrowConfirmation = bookID;
            LanguageToDisplayDuringBorrowConfirmation = book.Language;

           IsVisible_ToAddBorrow = true;
        }

        protected async Task OnDialogButtonClick_ToConfirmAddBorrow(int bookID, int userID)
        {
            try
            {
                var borrowToAddDTO = new BorrowToAddDTO();

                borrowToAddDTO.UserId = userID;
                borrowToAddDTO.BookId = bookID;

                var borrowDetailsDTO = await borrowService.AddBorrow(borrowToAddDTO);
                ReloadBookInBookDetails(borrowToAddDTO.BookId);
                IsVisible_ToAddBorrow = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void OnDialogButtonClick_ToCancelAddBorrow()
        {
            IsVisible_ToAddBorrow = false;
        }

        private BookDetailsDTO GetBook(int id)
        {
            return BookDetails.FirstOrDefault(i => i.Id == id);
        }

        private void ReloadBookInBookDetails(int id)
        {
            var bookToReload = GetBook(id);
            bookToReload.Status = "Unavailable";
        }

        protected async Task NavigateBackTo()
        {
            NavigationManager.NavigateTo($"/userBookSelection/{userId}");
        }

        protected async Task NavigateBackToUser()
        {
            NavigationManager.NavigateTo($"/displayuserdetails/{userId}");
        }

        public async void openModal_ToConfirmAddBorrow(int bookID)
        {
            var book = await BookService.GetBook(bookID);
            var bookDescription = await BookDescriptionInventoryService.GetBookDescription(book.BookDescriptionId);

            TitleToDisplayDurigBorrowConfirmation = bookDescription.Title;
            IDToDisplayDuringBorrowConfirmation = bookID;
            LanguageToDisplayDuringBorrowConfirmation = book.Language;


            IsOpened_ForAddBorrow = true;
        }

        public void cancelModal_ToConfirmAddBorrow()
        {
            IsOpened_ForAddBorrow = false;
        }

        public async Task confirmModal_ToConfirmAddBorrow()
        {

            try
            {
                var borrowToAddDTO = new BorrowToAddDTO();

                borrowToAddDTO.UserId = userId;
                borrowToAddDTO.BookId = IDToDisplayDuringBorrowConfirmation;

                var borrowDetailsDTO = await borrowService.AddBorrow(borrowToAddDTO);
                ReloadBookInBookDetails(borrowToAddDTO.BookId);

                User = await UserService.GetUser(userId);
                if (User.NumBorrowingBooks >= 5)
                {
                    NavigationManager.NavigateTo($"/displayuserdetails/{userId}");
                }

                IsOpened_ForAddBorrow = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
