using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Reflection.Emit;

namespace LibHub.Web.Pages
{
    public class HomePageBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IBorrowService BorrowService { get; set; }

        [Inject]
        protected IUserService UserService { get; set; }
        [Inject]
        protected IBookService BookService { get; set; }

        [Inject]
        protected IBookDescriptionInventoryService BookDescriptionService { get; set; }

        public SessionInfo[] BookInformation;
        public SessionInfo[] UserInformation;

        public int total_num_user { get; set; }
        public int total_num_books { get; set; }

        public int total_num_overdue_books_without_fines { get; set; }
        public int total_num_overdue_books_with_fines { get; set; }
        public int total_num_borrowed_books_not_overdue { get; set; }
        public int total_num_books_currently_available { get; set; }

        public int p_total_num_overdue_books_without_fines { get; set; }
        public int p_total_num_overdue_books_with_fines { get; set; }
        public int p_total_num_borrowed_books_not_overdue { get; set; }
        public int p_total_num_books_currently_available { get; set; }


        public int total_num_user_with_overdue_books_with_fines { get; set; }
        public int total_num_user_with_overdue_books_without_fines { get; set; }               
        public int total_num_user_without_overdue_books { get; set; }
        public int total_num_users_not_borrowing_books { get; set; }

        public int p_total_num_user_with_overdue_books_with_fines { get; set; }
        public int p_total_num_user_with_overdue_books_without_fines { get; set; }
        public int p_total_num_user_without_overdue_books { get; set; }
        public int p_total_num_users_not_borrowing_books { get; set; }

       
        public string ErrorMessage { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                var allBooks = await BookService.GetAllBooks();
                var allBorrowNotReturned = await BorrowService.GetAllBorrowsNotReturned();
                var users = await UserService.GetUsers();
                var UsersWithLateBorrowButNoFeesFined = await UserService.GetUsersWithLateBorrowButNoFeesFined();
                var UsersWithFeesFined = await UserService.GetUsersWithFeesFined();

                
                total_num_user = users.Count();
                total_num_books = allBooks.Count();

                total_num_overdue_books_without_fines = (allBorrowNotReturned.Where(b => (b.DueDate.Date < DateTime.Now.Date) && (!b.AreFeesFined))).Count();
                total_num_overdue_books_with_fines = (allBorrowNotReturned.Where(b => (b.DueDate.Date < DateTime.Now.Date) && (b.AreFeesFined))).Count();
                total_num_borrowed_books_not_overdue = (allBorrowNotReturned.Where(b => (b.DueDate.Date > DateTime.Now.Date))).Count();
                total_num_books_currently_available = (allBooks.Where(b => b.Status == "Available")).Count();

                p_total_num_overdue_books_without_fines = (total_num_overdue_books_without_fines / total_num_books) *100;
                p_total_num_overdue_books_with_fines = (total_num_overdue_books_with_fines / total_num_books) * 100 ;
                p_total_num_borrowed_books_not_overdue = (total_num_borrowed_books_not_overdue / total_num_books) * 100 ;
                p_total_num_books_currently_available = (total_num_books_currently_available / total_num_books) * 100 ;

                total_num_user_with_overdue_books_with_fines = UsersWithFeesFined.Count();
                total_num_user_with_overdue_books_without_fines = UsersWithLateBorrowButNoFeesFined.Count();
                total_num_user_without_overdue_books = (users.Where(u => u.NumBorrowingBooks > 0)).Count() - (total_num_user_with_overdue_books_without_fines + total_num_user_with_overdue_books_with_fines);
                total_num_users_not_borrowing_books = (users.Where(u => u.NumBorrowingBooks == 0)).Count();

                p_total_num_user_with_overdue_books_with_fines = (total_num_user_with_overdue_books_with_fines / total_num_user) * 100;
                p_total_num_user_with_overdue_books_without_fines = (total_num_user_with_overdue_books_without_fines / total_num_user) * 100;
                p_total_num_user_without_overdue_books = (total_num_user_without_overdue_books / total_num_user) * 100;
                p_total_num_users_not_borrowing_books = (total_num_users_not_borrowing_books / total_num_user) * 100;
                                
                BookInformation = GetBookInformation();
                UserInformation = GetUserInformation();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
        public void NavigateToUsersWithFeesFinedPage()
        {
            NavigationManager.NavigateTo("/UsersWithFeesFinedPage");
        }

        public void NavigateToUsersWithLateBorrowButNoFeesFinedPage()
        {
            NavigationManager.NavigateTo("/UsersWithLateBorrowButNoFeesFinedPage");
        }

        public void NavigateToAddBookDescriptionPage()
        {
            NavigationManager.NavigateTo("/addBookDescriptionPage");
        }

        public void NavigateToAddUserPage()
        {
            NavigationManager.NavigateTo("/addUserPage");
        }

        public class SessionInfo
        {
            public string Label { get; set; }
            public int Value { get; set; }
            public string ShowText { get; set; }
        }
                
        public SessionInfo[] GetBookInformation()
        {
            SessionInfo[] sales = new SessionInfo[] {
            new SessionInfo() { Label = $"Number of Overdue Books Without Fines Charged ({total_num_overdue_books_without_fines} books)", Value = total_num_overdue_books_without_fines, ShowText = $"{p_total_num_overdue_books_without_fines}%"},
            new SessionInfo() { Label = $"Number of Overdue Books With Fines Charged ({total_num_overdue_books_with_fines} books)", Value = total_num_overdue_books_with_fines, ShowText = $"{p_total_num_overdue_books_with_fines}%" },
            new SessionInfo() { Label = $"Number of Borrowed Books That Are Not Overdue ({total_num_borrowed_books_not_overdue} books)", Value = total_num_borrowed_books_not_overdue, ShowText = $"{p_total_num_borrowed_books_not_overdue}%" },
            new SessionInfo() { Label = $"Number of Books Currently Available ({total_num_books_currently_available} books)", Value = total_num_books_currently_available, ShowText = $"{p_total_num_books_currently_available}%" }
        };
            return sales;
        }
                
        public SessionInfo[] GetUserInformation()
        {
            SessionInfo[] sales = new SessionInfo[] {
            new SessionInfo() { Label = $"Number of Users With Overdue Books And Fines ({total_num_user_with_overdue_books_with_fines} users)", Value = total_num_user_with_overdue_books_with_fines, ShowText = $"{p_total_num_user_with_overdue_books_with_fines}%" },
            new SessionInfo() { Label = $"Number of Users With Overdue Books But No Fines ({total_num_user_with_overdue_books_without_fines} users)", Value = total_num_user_with_overdue_books_without_fines, ShowText = $"{p_total_num_user_with_overdue_books_without_fines}%" },
            new SessionInfo() { Label = $"Number of Users Borrowing With No Overdue Books ({total_num_user_without_overdue_books} users)", Value = total_num_user_without_overdue_books, ShowText = $"{p_total_num_user_without_overdue_books}%" },
            new SessionInfo() { Label = $"Number of Users Not Borrowing Books ({total_num_users_not_borrowing_books} users)", Value = total_num_users_not_borrowing_books, ShowText = $"{p_total_num_users_not_borrowing_books}%"}};
            return sales;
        }
    }
}
