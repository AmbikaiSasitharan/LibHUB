using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LibHub.Web.Pages
{
    public class DisplayUserBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }


        [Inject]
        public IUserService UserService { get; set; }

        public UserDetailsWIthLateBorrowDetailsDTO User { get; set; }

        [Inject]
        public IBorrowService BorrowService { get; set; }
        [Inject]
        public IRenewalService RenewalService { get; set; }

        [Inject]
        public IBookService bookService { get; set; }
        public List<BorrowDetailsDTO> BorrowDetails { get; set; }

        [Inject]
        public IRatingService ratingService { get; set; }

        public RatingToAddDTO ratingToAdd = new RatingToAddDTO();
        public IEnumerable<RatingDetailsDTO> AllUserRatings { get; set; }
        public int NumBorrowingBooks { get; set; }
        public string BookTitleToRate { get; set; }
        public string ErrorMessage { get; set; }
        public string TitleToDisplayDurigRenewConfirmation { get; set; }
        public int IDToDisplayDuringRenewConfirmation { get; set; }
        public DateTime OriginalDueDateToDisplayDuringRenewConfirmation { get; set; }
        public DateTime PotentialDueDateToDisplayDuringRenewConfirmation { get; set; }

        public bool IsVisisble_ToAddRenew = false;

        public bool IsVisible = false;

        public bool IsVisible_ToAddRatings = false;
        public bool IsOpened_ViewLogHistory = false;
        public bool IsOpened_ToAddRenew = false;
        public bool IsOpened_ToAddRating = false;
        public bool IsOpened_ToNotBeAbleToAddRating = false;

        public IEnumerable<LogEntryDTO> LogEntryHistory { get; set; }

        public async void confirmModal_ToAddRating()
        {
            try
            {
                var ratingDetailDTO = await ratingService.AddRating(ratingToAdd);
                IsOpened_ToAddRating = false;
            }
            catch (Exception)
            {

                throw;
            }

            IsOpened_ToAddRating = false;
        }

        public void cancelModal_ToAddRating()
        {
            IsOpened_ToAddRating = false;
        }

        public void openModal_ToAddRating()
        {
            IsOpened_ToAddRating = true;
        }

        public async void openModal_ToAddRenew(int borrowId)
        {
            var borrow = await BorrowService.GetBorrow(borrowId);

            TitleToDisplayDurigRenewConfirmation = borrow.BookTitle;
            IDToDisplayDuringRenewConfirmation = borrowId;
            OriginalDueDateToDisplayDuringRenewConfirmation = borrow.DueDate;
            PotentialDueDateToDisplayDuringRenewConfirmation = OriginalDueDateToDisplayDuringRenewConfirmation.AddDays(14);

            IsOpened_ToAddRenew = true;
        }


        public void cancelModal_ToAddRenew()
        {
            IsOpened_ToAddRenew = false;
        }

        public async void confirmModal_ToAddRenew(int borrowId)
        {
            try
            {
                var renew = await RenewalService.AddRenewal(borrowId);
                ReloadBookInBookDetails(borrowId);
                IsOpened_ToAddRenew = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void closeModal_ViewLogHistory()
        {
            IsOpened_ViewLogHistory = false;
        }

        public async void openModal_ViewLogHistory()
        {
            LogEntryHistory = await RenewalService.GetLogHistory(User.Id);
            IsOpened_ViewLogHistory = true;
        }

        public void OnDialogButtonClick()
        {
            IsVisible = false;
        }
        public void OnOpenButtonClick()
        {
            IsVisible = true;
        }
        public void OnDialogButtonClick_ToAddRating()
        {            
            IsVisible_ToAddRatings = false;
        }
        public void OnOpenButtonClick_ToAddRating()
        {
            IsVisible_ToAddRatings = true;
        }

        protected async Task AddRenew_Click(int borrowId)
        {
            var borrow = await BorrowService.GetBorrow(borrowId);

            TitleToDisplayDurigRenewConfirmation = borrow.BookTitle;
            IDToDisplayDuringRenewConfirmation = borrowId;
            OriginalDueDateToDisplayDuringRenewConfirmation = borrow.DueDate;
            PotentialDueDateToDisplayDuringRenewConfirmation = OriginalDueDateToDisplayDuringRenewConfirmation.AddDays(14);

            IsVisisble_ToAddRenew = true;
        }

        protected async Task OnDialogButtonClick_ToConfirmAddRenew(int borrowId)
        {
            try
            {
                var renew = await RenewalService.AddRenewal(borrowId);
                ReloadBookInBookDetails(borrowId);
                IsVisisble_ToAddRenew = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private BorrowDetailsDTO GetBorrowForReload(int id)
        {
            return BorrowDetails.FirstOrDefault(i => i.Id == id);
        }

        private void ReloadBookInBookDetails(int id)
        {
            var borrowToReload = GetBorrowForReload(id);
            borrowToReload.NumOfRevewals = borrowToReload.NumOfRevewals + 1;
            borrowToReload.DueDate = borrowToReload.DueDate.AddDays(14);
        }

        public void OnDialogButtonClick_ToCancelAddRenew()
        {
            IsVisisble_ToAddRenew = false;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                User = await UserService.GetUser(Id);
                BorrowDetails = await BorrowService.GetCurrentBorrowsOfAUser(User.Id);
                NumBorrowingBooks = User.NumBorrowingBooks;
                LogEntryHistory = await RenewalService.GetLogHistory(User.Id);
                AllUserRatings = await ratingService.GetAllRatingsOofAUser(User.Id);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
        
        /*protected async Task RemoveBorrow_Click(int id)
        {
            var borrow = await BorrowService.RemoveBorrow(id);

            RemoveBorrowFromBorrowDetails(id);
            NumBorrowingBooks = NumBorrowingBooks - 1;
        }*/

        private BorrowDetailsDTO GetBorrow(int id)
        {
            return BorrowDetails.FirstOrDefault(i => i.Id == id);
        }

        private void RemoveBorrowFromAllBorrowDetails(int id)
        {
            var borrowToRemove = GetBorrow(id);
            BorrowDetails.Remove(borrowToRemove);
        }

        protected async Task ReturnBorrow_Click(int id, BorrowDetailsDTO borrowDetailsDTO)
        {
            var borrow = await BorrowService.ReturnBorrow(id, borrowDetailsDTO);
            var allPastBorrows = await BorrowService.GetBorrowHistoryOfAUser(User.Id);
            User = await UserService.GetUser(Id);
            BookTitleToRate = borrowDetailsDTO.BookTitle;
            RemoveBorrowFromAllBorrowDetails(id);
            NumBorrowingBooks = NumBorrowingBooks - 1;

            if (AllUserRatings.Any(r => r.BookDescriptionId == borrowDetailsDTO.BookDescriptionId))
            {
                openModal_ToNotBeAbleToAddRating();
            }
            else
            {
                await SetRatingUserAndBookInfo(borrowDetailsDTO.BookId);
                openModal_ToAddRating();
            }
        }

        public void cancelModal_ToNotBeAbleToAddRating()
        {
            IsOpened_ToNotBeAbleToAddRating = false;
        }

        public void openModal_ToNotBeAbleToAddRating()
        {
            IsOpened_ToNotBeAbleToAddRating = true;
        }

        protected async Task SetRatingUserAndBookInfo(int bookId)
        {
            var BookToRate = new BookDetailsDTO();
            BookToRate = await bookService.GetBook(bookId);

            ratingToAdd.UserId = User.Id;
            ratingToAdd.BookDescriptionId = BookToRate.BookDescriptionId;
        }

        protected async Task AddRating_Click()
        {
            try
            {
                var ratingDetailDTO = await ratingService.AddRating(ratingToAdd);
                OnDialogButtonClick_ToAddRating();
            }
            catch (Exception)
            {

                throw;
            }        
        }

        public void NavigateToBrowseBooksForUser()
        {
            NavigationManager.NavigateTo($"/userBookSelection/{User.Id}");
        }
    }
}
