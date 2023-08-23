using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class DisplayBookDescriptionDetailsBase:ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IBookDescriptionInventoryService ProductService { get; set; }

        public BookDescriptionDetailsDTO Product { get; set; }

        public string GenresInOneString { get; set; }

        [Inject]
        public IBookService BookService { get; set; }

        public IEnumerable<BookDetailsDTO> BookDetails { get; set; }
        
        public BookToAddDTO bookToAdd = new BookToAddDTO();

        [Inject]
        public IRatingService ratingService { get; set; }

        public IEnumerable<RatingDetailsDTO> allRatingsForGivenBookDescription { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsVisible = false;

        public bool IsOpened_ForAddBookCopy = false;

        public async void OnDialogButtonClick()
        {
            bookToAdd.BookDescriptionId = Id;

            var bookDetailsDTO = await BookService.AddBook(bookToAdd);

            BookDetails = await BookService.GetBooksForBookDescription(Id);

            IsVisible = false;
        }

        private void close_addBook()
        {
            IsOpened_ForAddBookCopy = false;
        }

        public async Task closeModal_ForAddBookCopy()
        {
            close_addBook();

            var bookDetailsDTO =  await BookService.AddBook(bookToAdd);


            BookDetails = await BookService.GetBooksForBookDescription(Id);

            
        }

        public void cancelModal_ForAddBookCopy()
        {
            IsOpened_ForAddBookCopy = false;
        }

        public void openModal_ForAddBookCopy()
        {
            IsOpened_ForAddBookCopy = true;
        }

        public async void OnOpenButtonClick()
        {
            IsVisible = true;
        }

        private BookDetailsDTO GetBook(int id)
        {
            return BookDetails.FirstOrDefault(i => i.Id == id);
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                bookToAdd.BookDescriptionId = Id;
                Product = await ProductService.GetBookDescription(Id);
                BookDetails =(await BookService.GetBooksForBookDescription(Product.Id)).ToList();
                GenresInOneString = string.Join(",", Product.Genres);
                allRatingsForGivenBookDescription = await ratingService.GetRatingForBookDescription(Id);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
    }
}
