using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace LibHub.API.Repository
{
    public class RenewalRepository : IRenewalRepository
    {
        private readonly LibHubDbContext libHubDbContext;

        public RenewalRepository(LibHubDbContext libHubDbContext)
        {
            this.libHubDbContext = libHubDbContext;
        }

        public async Task<Renewal> AddRenewal(Borrow borrow)
        {
            var exisitingRenewal = await this.libHubDbContext.Renewals.FirstOrDefaultAsync(u =>
            u.BorrowId == borrow.Id &&
            u.EntryDate == DateTime.Now);

            if (exisitingRenewal == null)
            {
                var renewalToAdd = new Renewal
                {
                    Borrow = borrow,
                    BorrowId = borrow.Id,
                    OriginalDueDate = borrow.DueDate,
                    ChangedDueDate = (borrow.DueDate).AddDays(14),
                    EntryDate = DateTime.Now
                };

                var result = await this.libHubDbContext.Renewals.AddAsync(renewalToAdd);
                await this.libHubDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<Renewal>> GetAllRenewalsOfReturnedBorrows(int userId)
        {
            var borrows = await this.libHubDbContext.Renewals 
                                                     .Include(x => x.Borrow)
                                                     .ThenInclude(borrow => borrow.Book)
                                                     .ThenInclude(book => book.BookDescription)
                                                     .Where(i => (i.Borrow.UserId == userId) && (i.Borrow.IsReturned))
                                                     .ToListAsync();
            return borrows;
        }

        public async Task<Renewal> GetRenewal(int Id)
        {
            var renewal = await this.libHubDbContext.Renewals
                                             .FirstOrDefaultAsync(i => i.Id == Id);
            return renewal;
        }

        public async Task<Renewal> RemoveRenewal(int Id)
        {
            var renewalToRemove = await this.libHubDbContext.Renewals.FindAsync(Id);

            if (renewalToRemove != null)
            {
                this.libHubDbContext.Renewals.Remove(renewalToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return renewalToRemove;
        }
    }
}
