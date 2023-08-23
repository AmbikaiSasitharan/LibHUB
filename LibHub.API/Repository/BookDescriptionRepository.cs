using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Net;

namespace LibHub.API.Repository
{
    public class BookDescriptionRepository : IBookDescriptionRepository
    {
        private readonly LibHubDbContext libHubDbContext;
        private readonly IAuthorRepository authorRepository;
        private readonly IGenreRepository genreRepository;

        public BookDescriptionRepository(LibHubDbContext libHubDbContext, IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            this.libHubDbContext = libHubDbContext;
            this.authorRepository = authorRepository;
            this.genreRepository = genreRepository;
        }

        public async Task<BookDescription> GetBookDescription(int id)
        {
            var bookDescription = await libHubDbContext.BookDescriptions
                                                       .Include(x => x.Authors)
                                                       .Include(x => x.Genres)
                                                       .Include(x => x.Ratings)
                                                       .Include(x => x.Books)
                                                       .FirstOrDefaultAsync(i => i.Id == id);
            return bookDescription;
        }

        public async Task<IEnumerable<BookDescription>> GetBookDescriptions()
        {
            var bookDescriptions = await this.libHubDbContext.BookDescriptions
                                                             .Include(x => x.Authors)
                                                             .Include(x => x.Ratings)
                                                             .ToListAsync();
            return bookDescriptions;
        }

        public async Task<BookDescription> SubtractOneFromNumAvailable(int id)
        {
            var bookDescription = await this.libHubDbContext.BookDescriptions.FindAsync(id);

            if (bookDescription != null)
            {
                bookDescription.NumAvailable = bookDescription.NumAvailable - 1;
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescription;
            }

            return null;
        }

        public async Task<BookDescription> AddOneToNumAvailable(int id)
        {
            var bookDescription = await this.libHubDbContext.BookDescriptions.FindAsync(id);

            if (bookDescription != null)
            {
                bookDescription.NumAvailable = bookDescription.NumAvailable + 1;
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescription;
            }

            return null;
        }

        public async Task<BookDescription> RemoveAuthorFromBookDescription(int Id, int authorId)
        {
            var bookDescriptionToRemoveFrom = await this.libHubDbContext.BookDescriptions.FindAsync(Id);
            var authorToRemove = await this.libHubDbContext.Authors.FindAsync(authorId);

            if ((bookDescriptionToRemoveFrom != null) || (authorToRemove != null))
            {
                bookDescriptionToRemoveFrom.Authors.Remove(authorToRemove);
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescriptionToRemoveFrom;
            }

            return null;
        }

        public async Task<BookDescription> AddAuthorToBookDescription(int Id, int authorId)
        {
            var bookDescriptionToAddTo = await this.libHubDbContext.BookDescriptions.FindAsync(Id);
            var authorToAdd = await this.libHubDbContext.Authors.FindAsync(authorId);

            if ((bookDescriptionToAddTo == null) || (authorToAdd == null))
            {
                return null;
            }

            bookDescriptionToAddTo.Authors.Add(authorToAdd);
            await this.libHubDbContext.SaveChangesAsync();
            return bookDescriptionToAddTo;
        }

        public async Task<BookDescription> AddBookDescription(BookDescriptionToAddDTO bookDescriptionToAddDTO)
        {
            var existingBookDescrption = await this.libHubDbContext.BookDescriptions.Where(u =>
                                                                                     u.Title == bookDescriptionToAddDTO.Title &&
                                                                                     u.PublishDate.Date == bookDescriptionToAddDTO.PublishDate.Date)
                                                                                    .FirstOrDefaultAsync();

            if (existingBookDescrption == null)
            {
                var bookDescriptionToAdd = new BookDescription
                {
                    Title = bookDescriptionToAddDTO.Title,
                    PublishDate = bookDescriptionToAddDTO.PublishDate,
                    Ratings = new List<Rating>(),
                    Rating = 0.0F,
                    NumRatings = 0.0F,
                    NumCopies = 0,
                    NumAvailable = 0,
                    Books = new List<Book>(),
                    Authors = new List<Author>(),
                    Genres = new List<Genre>(),
                    Description = bookDescriptionToAddDTO.Description,
                    EntryDate = DateTime.Now
                };

                var result = await this.libHubDbContext.BookDescriptions.AddAsync(bookDescriptionToAdd);
                await this.libHubDbContext.SaveChangesAsync();

                return result.Entity;
            }

            return null;
        }

        public async Task<BookDescription> AddGenreToBookDescription(int Id, int genreId)
        {
            var bookDescriptionToAddTo = await this.libHubDbContext.BookDescriptions.FindAsync(Id);
            var GenreToAdd = await this.libHubDbContext.Genres.FindAsync(genreId);

            if ((bookDescriptionToAddTo == null) || (GenreToAdd == null))
            {
                return null;
            }

            bookDescriptionToAddTo.Genres.Add(GenreToAdd);
            await this.libHubDbContext.SaveChangesAsync();
            return bookDescriptionToAddTo;
        }

        public async Task<BookDescription> AddBookToBookDescription(int Id, int bookId)
        {
            var bookDescriptionToAddTo = await this.libHubDbContext.BookDescriptions.FindAsync(Id);
            var BookToAdd = await this.libHubDbContext.Books.FindAsync(bookId);

            if ((bookDescriptionToAddTo != null) || (BookToAdd != null))
            {
                bookDescriptionToAddTo.Books.Add(BookToAdd);
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescriptionToAddTo;
            }

            return null;
        }

        public async Task<BookDescription> RemoveBookDescription(int Id)
        {
            var bookDescriptionToRemove = await this.libHubDbContext.BookDescriptions.FindAsync(Id);

            if (bookDescriptionToRemove != null)
            {
                this.libHubDbContext.BookDescriptions.Remove(bookDescriptionToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return bookDescriptionToRemove;
        }

        public async Task<BookDescription> SubtractOneFromNumCopies(int id)
        {
            var bookDescription = await this.libHubDbContext.BookDescriptions.FindAsync(id);

            if (bookDescription != null)
            {
                bookDescription.NumCopies = bookDescription.NumCopies - 1;
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescription;
            }

            return null;
        }

        public async Task<BookDescription> AddOneToNumCopies(int id)
        {
            var bookDescription = await this.libHubDbContext.BookDescriptions.FindAsync(id);

            if (bookDescription != null)
            {
                bookDescription.NumCopies = bookDescription.NumCopies + 1;
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescription;
            }

            return null;
        }

        public async Task<BookDescription> RemoveBookFromBookDescription(int Id, int bookId)
        {
            var bookDescriptionToRemoveFrom = await this.libHubDbContext.BookDescriptions.FindAsync(Id);
            var bookToRemove = await this.libHubDbContext.Books.FindAsync(bookId);

            if ((bookDescriptionToRemoveFrom != null) || (bookToRemove != null))
            {
                bookDescriptionToRemoveFrom.Books.Remove(bookToRemove);
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescriptionToRemoveFrom;
            }

            return null;
        }

        public async Task<BookDescription> AddRatingToBookDescription(int Id, float ratingToAdd)
        {
            var bookDescriptionToAddTo = await this.libHubDbContext.BookDescriptions.FindAsync(Id);

            if (bookDescriptionToAddTo != null)
            {
                bookDescriptionToAddTo.NumRatings = bookDescriptionToAddTo.NumRatings + 1;
                bookDescriptionToAddTo.Rating = (bookDescriptionToAddTo.Rating + ratingToAdd) /bookDescriptionToAddTo.NumRatings;
                await this.libHubDbContext.SaveChangesAsync();
            }

            return bookDescriptionToAddTo;
        }

        public async Task<BookDescription> RemoveRatingFromBookDescription(int Id, float ratingToRemove)
        {
            var bookDescriptionToRemoveFrom = await this.libHubDbContext.BookDescriptions.FindAsync(Id);

            if (bookDescriptionToRemoveFrom != null)
            {
                bookDescriptionToRemoveFrom.NumRatings = bookDescriptionToRemoveFrom.NumRatings - 1;
                bookDescriptionToRemoveFrom.Rating = (bookDescriptionToRemoveFrom.Rating - ratingToRemove) / bookDescriptionToRemoveFrom.NumRatings;
                await this.libHubDbContext.SaveChangesAsync();
            }

            return bookDescriptionToRemoveFrom;
        }

        public async Task<Genre> RemoveGenreFromBookDescriptions(List<BookDescription> bookDescriptions, int genreId)
        {
            var genreToRemove = await this.libHubDbContext.Genres.FindAsync(genreId);

            if (genreToRemove != null)
            {
                for (var i = 0; i < bookDescriptions.Count; i++)
                {
                    var bookDescriptionToRemoveFrom = await this.libHubDbContext.BookDescriptions.FindAsync((bookDescriptions[i]).Id);

                    if (bookDescriptionToRemoveFrom != null)
                    {
                        bookDescriptionToRemoveFrom.Genres.Remove(genreToRemove);
                        await this.libHubDbContext.SaveChangesAsync();
                    }

                    return genreToRemove;
                }
            }

            return genreToRemove;
        }

        public async Task<Author> RemoveAuthorFromBookDescriptions(List<BookDescription> bookDescriptions, int authorId)
        {
            var authorToRemove = await this.libHubDbContext.Authors.FindAsync(authorId);

            if (authorToRemove != null)
            {
                for (var i = 0; i < bookDescriptions.Count; i++)
                {
                    var bookDescriptionToRemoveFrom = await this.libHubDbContext.BookDescriptions.FindAsync((bookDescriptions[i]).Id);

                    if (bookDescriptionToRemoveFrom != null)
                    {
                        bookDescriptionToRemoveFrom.Authors.Remove(authorToRemove);
                        await this.libHubDbContext.SaveChangesAsync();
                    }

                    return authorToRemove;
                }
            }

            return authorToRemove;
        }

        public async Task<BookDescription> DeactivateBookDescription(int Id)
        {
            var bookDescription = await this.libHubDbContext.BookDescriptions.FindAsync(Id);

            if (bookDescription != null)
            {
                bookDescription.IsActive = false;
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescription;
            }

            return null;
        }

        public async Task<BookDescription> ActivateBookDescription(int Id)
        {
            var bookDescription = await this.libHubDbContext.BookDescriptions.FindAsync(Id);

            if (bookDescription != null)
            {
                bookDescription.IsActive = true;
                await this.libHubDbContext.SaveChangesAsync();
                return bookDescription;
            }

            return null;
        }
    }
}