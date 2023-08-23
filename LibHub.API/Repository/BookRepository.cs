using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace LibHub.API.Repository
{
    public class BookRepository: IBookRepository
    {
        private readonly LibHubDbContext libHubDbContext;

        public BookRepository(LibHubDbContext libHubDbContext)
        {
            this.libHubDbContext = libHubDbContext;
        }

        public async Task<IEnumerable<Book>> GetBooksForBookDescription(int bookDescriptionId)
        {
            var books = await this.libHubDbContext.Books
                                             .Where(i => i.BookDescriptionId == bookDescriptionId)
                                             .ToListAsync();
            return books;
        }

        public async Task<Book> GetBook(int id)
        {
            var book = await this.libHubDbContext.Books
                                             .FirstOrDefaultAsync(i => i.Id == id);
            return book;
        }

        public async Task<Book> ChangeStatusToUnavailable(int id)
        {
            var book = await this.libHubDbContext.Books.FindAsync(id);

            if (book != null)
            {
                book.Status = "Unavailable";
                await this.libHubDbContext.SaveChangesAsync();
                return book;
            }

            return null;
        }

        public async Task<Book> ChangeStatusToAvailable(int id)
        {
            var book = await this.libHubDbContext.Books.FindAsync(id);

            if (book != null)
            {
                book.Status = "Available";
                await this.libHubDbContext.SaveChangesAsync();
                return book;
            }

            return null;
        }

        public async Task<Book> AddBook(BookToAddDTO bookToAddDTO, BookDescription bookDescription)
        {
            var book = await (from bookDescriptionBookIsCopyOf in this.libHubDbContext.BookDescriptions
                              where bookDescriptionBookIsCopyOf.Id == bookDescription.Id
                              select new Book
                              {
                                  BookDescription = bookDescription,
                                  BookDescriptionId = bookDescription.Id,
                                  Status = "Available",
                                  Language = bookToAddDTO.Language, 
                                  Users = new List<Borrow>(),
                                  EntryDate = DateTime.Now,
                                  CostAtTimeOfPurchase = bookToAddDTO.CostAtTimeOfPurchase
                              }).SingleOrDefaultAsync();
            if (book != null)
            {
                var result = await this.libHubDbContext.Books.AddAsync(book);
                await this.libHubDbContext.SaveChangesAsync();
                return result.Entity;
            }

            return book;
        }

        public async Task<Book> RemoveBook(int Id)
        {
            var bookToRemove = await this.libHubDbContext.Books.FindAsync(Id);

            if (bookToRemove != null)
            {
                this.libHubDbContext.Books.Remove(bookToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return bookToRemove;
        }

        public async Task<IEnumerable<Book>> GetAllBooksForLibraryStats()
        {
            var books = await this.libHubDbContext.Books
                                                        .ToListAsync();
            return books;
        }
    }
}