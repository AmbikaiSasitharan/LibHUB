using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace LibHub.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LibHubDbContext libHubDbContext;
        public UserRepository(LibHubDbContext libHubDbContext)
        {
            this.libHubDbContext = libHubDbContext;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await libHubDbContext.Users
                                            .Include(x => x.Borrows)
                                            .FirstOrDefaultAsync(i => i.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await this.libHubDbContext.Users.ToListAsync();
            return users;
        }

        public async Task<User> AddOneToNumBorrowingBooks(int id)
        {
            var user = await this.libHubDbContext.Users.FindAsync(id);

            if (user != null)
            {
                user.NumBorrowingBooks = user.NumBorrowingBooks + 1;
                await this.libHubDbContext.SaveChangesAsync();
                return user;
            }

            return null;
        }

        public async Task<User> SubtractOneFromNumBorrowingBooks(int id)
        {
            var user = await this.libHubDbContext.Users.FindAsync(id);
            
            if (user != null)
            {
                user.NumBorrowingBooks = user.NumBorrowingBooks - 1;
                await this.libHubDbContext.SaveChangesAsync();
                return user;
            }

            return null;
        }

        public async Task<User> AddUser(UserToAddDTO userToAddDTO)
        {
            var existingUser = await this.libHubDbContext.Users.FirstOrDefaultAsync(u =>
            u.Email == userToAddDTO.Email &&
            u.FName == userToAddDTO.FName &&
            u.MName == userToAddDTO.MName &&
            u.LName == userToAddDTO.LName &&
            u.BirthDate == userToAddDTO.BirthDate);

            if (existingUser == null)
            {
                var userToAdd = new User
                {
                    UserName = userToAddDTO.UserName,
                    FName = userToAddDTO.FName,
                    MName = userToAddDTO.MName,
                    LName = userToAddDTO.LName,
                    Email = userToAddDTO.Email,
                    Address = userToAddDTO.Address,
                    PhoneNum = userToAddDTO.PhoneNum,
                    BirthDate = userToAddDTO.BirthDate,
                    Borrows = new List<Borrow>(),
                    Ratings = new List<Rating>(),
                    NumBorrowingBooks = 0,
                    EntryDate = DateTime.Now
                };

                var result = await this.libHubDbContext.Users.AddAsync(userToAdd);
                await this.libHubDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<User> RemoveUser(int Id)
        {
            var userToRemove = await this.libHubDbContext.Users.FindAsync(Id);

            if (userToRemove != null)
            {
                this.libHubDbContext.Users.Remove(userToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return userToRemove;
        }

        public async Task<User> UpdateUserInformation(int id, InfoToUpdateUserDTO infoToUpdateUserDTO)
        {
            var user = await this.libHubDbContext.Users.FindAsync(id);

            if (user != null)
            {
                user.FName = infoToUpdateUserDTO.FName;
                user.MName = infoToUpdateUserDTO.MName;
                user.LName = infoToUpdateUserDTO.LName;
                user.Email = infoToUpdateUserDTO.Email;
                user.Address = infoToUpdateUserDTO.Address;
                user.PhoneNum = infoToUpdateUserDTO.PhoneNum;
                await this.libHubDbContext.SaveChangesAsync();
                return user;
            }

            return null;
        }

        public async Task<User> DeactivateUser(int Id)
        {
            var user = await this.libHubDbContext.Users.FindAsync(Id);

            if (user != null)
            {
                user.IsActive = false;
                await this.libHubDbContext.SaveChangesAsync();
                return user;
            }

            return null;
        }

        public async Task<User> ActivateUser(int Id)
        {
            var user = await this.libHubDbContext.Users.FindAsync(Id);

            if (user != null)
            {
                user.IsActive = true;
                await this.libHubDbContext.SaveChangesAsync();
                return user;
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetUsersWithLateBorrows()
        {
            var usersWithLateBorrows = await this.libHubDbContext.Users
                                                                        .Include(i => i.Borrows)
                                                                        .Where(user => user.Borrows.Any(borrow =>
                                                                                                                    (borrow.DueDate).Date < (DateTime.Now).Date &&
                                                                                                                    !borrow.IsReturned))
                                                                        .Where(u => u.IsActive == true)
                                                                        .Select(user => new User
                                                                                                {
                                                                                                    Id = user.Id,
                                                                                                    UserName = user.UserName,
                                                                                                    FName = user.FName,
                                                                                                    MName = user.MName,
                                                                                                    LName = user.LName,
                                                                                                    Email = user.Email,
                                                                                                    Address = user.Address,
                                                                                                    PhoneNum = user.PhoneNum,
                                                                                                    BirthDate = user.BirthDate,
                                                                                                    Borrows = user.Borrows.Where(borrow =>
                                                                                                                                (borrow.DueDate).Date < (DateTime.Now).Date &&
                                                                                                                                !borrow.IsReturned).ToList(),
                                                                                                    Ratings = user.Ratings,
                                                                                                    NumBorrowingBooks = user.NumBorrowingBooks,
                                                                                                    IsActive = user.IsActive,
                                                                                                    EntryDate = user.EntryDate
                                                                        })
                                                                        .ToListAsync();

            return usersWithLateBorrows;
        }

        public async Task<IEnumerable<User>> GetAllUsersForLibraryStats()
        {
            var users = await this.libHubDbContext.Users
                                                        .Include(x => x.Borrows)
                                                        .ToListAsync();
            return users;
        }
    }
}

//&& ((DateTime.Now).Date - (borrow.DueDate).Date).Days <= 7)