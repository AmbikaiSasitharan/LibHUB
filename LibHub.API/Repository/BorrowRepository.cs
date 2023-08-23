using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace LibHub.API.Repository
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibHubDbContext libHubDbContext;
        private readonly IBookRepository bookRepository;
        private readonly IUserRepository userRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;

        public BorrowRepository(LibHubDbContext libHubDbContext, IBookRepository bookRepository, IUserRepository userRepository, IBookDescriptionRepository bookDescriptionRepository)
        {
            this.libHubDbContext = libHubDbContext;
            this.bookRepository = bookRepository;
            this.userRepository = userRepository;
            this.bookDescriptionRepository = bookDescriptionRepository;
        }

        private async Task<bool> BorrowExists(int bookId, int userId, DateTime entryDate)
        {
            return await this.libHubDbContext.Borrows.AnyAsync(c => c.BookId ==  bookId 
                                                                    && c.UserId == userId 
                                                                    && (c.EntryDate) == entryDate);
        }

        public async Task<Borrow> AddBorrow(BorrowToAddDTO borrowToAddDTO, Book book, User user)
        {
            if (await BorrowExists(borrowToAddDTO.BookId, borrowToAddDTO.UserId, DateTime.Now) == false)
            {
                var borrow = await (from searchBook in this.libHubDbContext.Books
                                    where searchBook.Id == borrowToAddDTO.BookId
                                    select new Borrow
                                    {
                                        DueDate = (DateTime.Now).AddDays(14),
                                        UserId = borrowToAddDTO.UserId,
                                        User = user,
                                        BookId = borrowToAddDTO.BookId,
                                        Book = book,
                                        IsReturned = false,
                                        DateOfReturn = new DateTime(),
                                        NumOfRevewals = 0,
                                        EntryDate = DateTime.Now,
                                        Renewals = new List<Renewal>()
                                    }
                                ).SingleOrDefaultAsync();
                if (borrow != null)
                {
                    var result = await this.libHubDbContext.Borrows.AddAsync(borrow);
                    await this.libHubDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<Borrow> RemoveBorrow(int Id)
        {
            var borrowToRemove = await this.libHubDbContext.Borrows.FindAsync(Id);

            if (borrowToRemove != null)
            {
                this.libHubDbContext.Borrows.Remove(borrowToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return borrowToRemove;
        }

        public async Task<Borrow> GetBorrow(int Id)
        {
            var borrow = await libHubDbContext.Borrows
                                              .Include(x => x.Book)
                                              .ThenInclude(book => book.BookDescription)
                                              .Include(x => x.Renewals)
                                              .FirstOrDefaultAsync(i => i.Id == Id);
            return borrow;
        }

        public async Task<IEnumerable<Borrow>> GetBorrowHistoryOfAUser(int UserId)
        {
            var borrows = await this.libHubDbContext.Borrows
                                                    .Include(x => x.Book)
                                                    .ThenInclude(book => book.BookDescription)
                                                    .Include(x => x.Renewals)
                                                    .Where(i => i.UserId == UserId)
                                                    .ToListAsync();
            return borrows;
        }

        public async Task<IEnumerable<Borrow>> RemovwAllBorrowsOfAUser(int userId)
        {
            var borrowsToRemove = await this.libHubDbContext.Borrows
                                                    .Include(x => x.Book)
                                                    .ThenInclude(book => book.BookDescription)
                                                    .Include(x => x.Renewals)
                                                    .Where(i => i.UserId == userId)
                                                    .ToListAsync();
            if (borrowsToRemove != null)
            {
                foreach(Borrow borrow in borrowsToRemove)
                {
                    if (borrow.IsReturned == false)
                    {
                        var bookDescriptionToUpdate = await this.bookDescriptionRepository.AddOneToNumAvailable(borrow.Book.BookDescriptionId);
                        var bookToUpdate = await this.bookRepository.ChangeStatusToAvailable(borrow.BookId);
                        var userToUpdate = await this.userRepository.SubtractOneFromNumBorrowingBooks(borrow.UserId);

                        if ((bookDescriptionToUpdate == null) || (bookToUpdate == null) || (userToUpdate == null) || (borrow == null))
                        {
                            return null;
                        }

                        await ReturnBorrow(borrow.Id);

                    }
                    
                    this.libHubDbContext.Borrows.Remove(borrow);
                    await this.libHubDbContext.SaveChangesAsync();
                }
            }

            return borrowsToRemove;
        }

        public async Task<IEnumerable<Borrow>> GetCurrentBorrowsOfAUser(int UserId)
        {
            var borrows = await this.libHubDbContext.Borrows
                                                    .Include(x => x.Book)
                                                    .ThenInclude(book => book.BookDescription)
                                                    .Include(x => x.Renewals)
                                                    .Where(i => (i.UserId == UserId) && (i.IsReturned == false))
                                                    .ToListAsync();
            return borrows;
        }

        public async Task<Borrow> ReturnBorrow(int Id)
        {
            var borrow = await this.libHubDbContext.Borrows.FindAsync(Id);

            if (borrow != null)
            {
                borrow.IsReturned = true;
                borrow.DateOfReturn = DateTime.Now;
                await this.libHubDbContext.SaveChangesAsync();
                return borrow;
            }
            return null;
        }

        public async Task<Borrow> AddRenewalToBorrow(int Id, int renewalId)
        {
            var borrowToAddTo = await this.libHubDbContext.Borrows.FindAsync(Id);
            var renewalToAdd = await this.libHubDbContext.Renewals.FindAsync(renewalId);

            if ((borrowToAddTo == null) || (renewalToAdd == null))
            {
                return null;
            }

            borrowToAddTo.Renewals.Add(renewalToAdd);
            await this.libHubDbContext.SaveChangesAsync();
            return borrowToAddTo;
        }

        public async Task<Borrow> RemoveRenewalFromBorrow(int Id, int renewalId)
        {
            var borrowToRemoveFrom = await this.libHubDbContext.Borrows.FindAsync(Id);
            var renewalToRemove = await this.libHubDbContext.Renewals.FindAsync(renewalId);

            if ((borrowToRemoveFrom != null) || (renewalToRemove != null))
            {
                borrowToRemoveFrom.Renewals.Remove(renewalToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return borrowToRemoveFrom;
        }

        public async Task<Borrow> AddOneToNumRenewals(int Id)
        {
            var borrow = await this.libHubDbContext.Borrows.FindAsync(Id);

            if (borrow != null)
            {
                borrow.NumOfRevewals = borrow.NumOfRevewals + 1;
                await this.libHubDbContext.SaveChangesAsync();
                return borrow;
            }

            return null;
        }

        public async Task<Borrow> SubtractOneFromNumRenewals(int Id)
        {
            var borrow = await this.libHubDbContext.Borrows.FindAsync(Id);

            if (borrow != null)
            {
                borrow.NumOfRevewals = borrow.NumOfRevewals - 1;
                await this.libHubDbContext.SaveChangesAsync();
                return borrow;
            }

            return null;
        }

        public async Task<IEnumerable<Borrow>> GetLateBorrowOfAUser(int UserId)
        {
            var borrows = await this.libHubDbContext.Borrows
                                                    .Include(x => x.Book)
                                                    .ThenInclude(book => book.BookDescription)
                                                    .Include(x => x.Renewals)
                                                    .Where(i => (i.UserId == UserId) && (i.IsReturned == false) && ((i.DueDate).Date < (DateTime.Now).Date))
                                                    .ToListAsync();
            return borrows;
        }

        public async Task<Borrow> Add14DaysToDueDate(int Id)
        {
            var borrow = await this.libHubDbContext.Borrows.FindAsync(Id);

            if (borrow != null)
            {
                borrow.DueDate = (borrow.DueDate).AddDays(14);
                await this.libHubDbContext.SaveChangesAsync();
                return borrow;
            }

            return null;
        }

        public async Task<IEnumerable<Borrow>> GetAllReturnedBorrows(int userId)
        {
            var borrows = await this.libHubDbContext.Borrows
                                                     .Include(x => x.Book)
                                                     .ThenInclude(book => book.BookDescription)
                                                     .Include(x => x.Renewals)
                                                     .Where(i => (i.UserId == userId) && (i.IsReturned))
                                                     .ToListAsync();
            return borrows;
        }
        
        public async Task<IEnumerable<Borrow>> GetAllBorrowsForLibraryStats()
        {
            var borrows = await this.libHubDbContext.Borrows
                .Include(x => x.Renewals)
                .Include(x => x.Book)
                .ThenInclude(x2 => x2.BookDescription)
                                                            .Where(b => !b.IsReturned)
                                                            .ToListAsync();
            return borrows;
        }

        public async Task<Borrow> ChangeIsLateNotified(int borrowId)
        {

            var borrowToChange = await this.libHubDbContext.Borrows.FindAsync(borrowId);
                if (borrowToChange != null)
                {
                    borrowToChange.IsLateNotified = true;
                    await this.libHubDbContext.SaveChangesAsync();
                return borrowToChange;
                }
            return borrowToChange;

        }


        public async Task<Borrow> ChangeIsFineNotified(int borrowId)
        {
            var borrowToChange = await this.libHubDbContext.Borrows.FindAsync(borrowId);
            if (borrowToChange != null)
            {
                borrowToChange.IsFineNotified = true;
                await this.libHubDbContext.SaveChangesAsync();
                return borrowToChange;
            }

            return borrowToChange;
        }
    }
}
