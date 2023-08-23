using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace LibHub.Web.Pages
{
    public class UsersWithLateBorrowButNoFeesFinedBase :ComponentBase
    {
        [Inject]
        IUserService UserService { get; set; }
        public List<UserWithLateBorrowsDTO> usersWithLateBorrows { get; set; }

        [Inject]
        IEmailService EmailService { get; set; }

        [Inject]
        IBorrowService borrowService { get; set; }  
        protected override async Task OnInitializedAsync()
        {
            usersWithLateBorrows = (await UserService.GetUsersWithLateBorrowButNoFeesFined()).ToList();
        }

        public async Task Notify_AllUsers()
        {
            for (var i = 0; i < usersWithLateBorrows.Count(); i++)
            {
                await Notify_User(usersWithLateBorrows[i]);
            }
        }

        public async Task Notify_User(UserWithLateBorrowsDTO user)
        {
            string borrowListString = "";
            foreach (BorrowDetailsDTO borrow in user.borrows)
            {
                borrowService.UpdateBorrowIsLateNotified(borrow.Id);
                var borrowToAdd = $"<div>Borrow ID:{borrow.Id}, Book ID:{borrow.BookId}, Book Title:{borrow.BookTitle},  {borrow.NumberOfDaysTillFeesAreApplied} days till fees are applied </div>";
                borrowListString = borrowListString + borrowToAdd;
            }
            var emailDTO = new EmailDTO
            {
                To = "verner22@ethereal.email",
                Subject = "Library Late Notice",
                Body = 
                $"<div>Dear {user.FullName}, </div>" +
                $"<div>This is a notification from our library to inform you that you have the following book(s) overdue, and unless you return these book within the given number of days, a fine will be charged. Here is a list of your overdue books: </div>" +
                borrowListString
            };

            try
            {
                EmailService.SendEmail(emailDTO);
                RemoveUserFromAllUnNotfiedUsersList(user.UserId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void RemoveUserFromAllUnNotfiedUsersList(int id)
        {
            var userToRemove = usersWithLateBorrows.FirstOrDefault(i => i.UserId == id);
            usersWithLateBorrows.Remove(userToRemove);
        }
    }
}
