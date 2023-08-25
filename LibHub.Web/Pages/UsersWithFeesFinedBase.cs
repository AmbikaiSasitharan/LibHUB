using LibHub.Models.DTOs;
using LibHub.Web.Services;
using LibHub.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace LibHub.Web.Pages
{
    public class UsersWithFeesFinedBase:ComponentBase
    {
        [Inject]
        IUserService UserService { get; set; }

        [Inject]
        IEmailService emailService { get; set; }

        [Inject]
        IBorrowService borrowService { get; set; }
        public List<UserWithLateBorrowsDTO> usersWithLateBorrows { get; set; }
        protected override async Task OnInitializedAsync()
        {
            usersWithLateBorrows = (await UserService.GetUsersWithFeesFined()).ToList();
        }

        public async Task Notify_AllUsers ()
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
                if (borrow.AreFeesFined)
                {
                    var borrowToAdd = $"<div>Borrow ID:{borrow.Id}, {borrow.BookTitle}, Book ID:{borrow.BookId}, {borrow.Cost.ToString("C", new CultureInfo("en-US"))}</div>";
                    borrowListString = borrowListString + borrowToAdd;
                    borrowService.UpdateBorrowIsFineNotified(borrow.Id);

                }
                else
                {
                    var borrowToAdd = $"<div>Borrow ID:{borrow.Id}, {borrow.BookTitle}, Book ID:{borrow.BookId}, {borrow.NumberOfDaysTillFeesAreApplied} days till fees are applied</div>";
                    borrowListString = borrowListString + borrowToAdd;
                    borrowService.UpdateBorrowIsLateNotified(borrow.Id);
                }
            }
            var emailDTO = new EmailDTO
            {
                To = user.Email,
                Subject = "Library Late Notice",
                Body =
                $"<div>Dear {user.FullName}, </div>" +
                $"<div>This is a notification from our library to inform you that you have the following book(s) overdue, and at least one of which are now a fined as the one week probation is over. </div>" +
                borrowListString
            };

            try
            {
                emailService.SendEmail(emailDTO);
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
