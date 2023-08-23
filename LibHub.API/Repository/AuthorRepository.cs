using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace LibHub.API.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibHubDbContext libHubDbContext;

        public AuthorRepository(LibHubDbContext libHubDbContext)
        {
            this.libHubDbContext = libHubDbContext;
        }
        public async Task<Author> AddAuthor(AuthorToAddDTO authorToAdd)
        {
            var existingAuthor = await this.libHubDbContext.Authors.FirstOrDefaultAsync(u => u.FName == authorToAdd.FName &&
                                                                                             u.MName == authorToAdd.MName &&
                                                                                             u.LName == authorToAdd.LName);

            if (existingAuthor == null)
            {

                var author = new Author
                {
                    FName = authorToAdd.FName,
                    MName = authorToAdd.MName,
                    LName = authorToAdd.LName,
                    FullName = authorToAdd.FullName,
                    BookDescriptions = new List<BookDescription>(),
                    EntryDate = DateTime.Now
                };

                var result = await this.libHubDbContext.Authors.AddAsync(author);
                await this.libHubDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<Author> AddBookDescriptionToAuthor(int Id, int bookDescriptionId)
        {
            var authorToAddTo = await this.libHubDbContext.Authors.FindAsync(Id);
            var bookDescriptionToAdd = await this.libHubDbContext.BookDescriptions.FindAsync(bookDescriptionId);

            if ((authorToAddTo != null) || (bookDescriptionToAdd != null))
            {
                authorToAddTo.BookDescriptions.Add(bookDescriptionToAdd);
                await this.libHubDbContext.SaveChangesAsync();
                return authorToAddTo;
            }

            return null;
        }

        public async void AddBookDescriptionToAuthorGivenEntities(Author author, BookDescription bookDescription)
        {
            author.BookDescriptions.Add(bookDescription);
            await this.libHubDbContext.SaveChangesAsync();            
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            var authors = await this.libHubDbContext.Authors
                                                   .Include(x => x.BookDescriptions)
                                                   .ToListAsync();
            return authors;
        }

        public async Task<Author> GetAuthor(int Id)
        {
            var author = await this.libHubDbContext.Authors
                                                   .Include(x => x.BookDescriptions)
                                                   .FirstOrDefaultAsync(i => i.Id == Id);
            return author;
        }

        public async Task<Author> RemoveAuthor(int Id)
        {
            var authorToRemove = await this.libHubDbContext.Authors.FindAsync(Id);

            if (authorToRemove != null)
            {
                this.libHubDbContext.Authors.Remove(authorToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return authorToRemove;
        }

        public async Task<Author> RemoveBookDescriptionFromAuthor(int Id, int bookDescriptionId)
        {
            var authorToRemoveFrom = await this.libHubDbContext.Authors.FindAsync(Id);
            var bookDescriptionToRemove = await this.libHubDbContext.BookDescriptions.FindAsync(bookDescriptionId);

            if ((authorToRemoveFrom != null) || (bookDescriptionToRemove != null))
            {
                authorToRemoveFrom.BookDescriptions.Remove(bookDescriptionToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return authorToRemoveFrom;
        }

        public async Task<BookDescription> RemoveBookDescriptionFromAuthors(List<Author> authors, int bookDescriptionId)
        {
            var bookDescriptionToRemove = await this.libHubDbContext.BookDescriptions.FindAsync(bookDescriptionId);
            if (bookDescriptionToRemove != null)
            {
                for (var i = 0; i < authors.Count; i++)
                {
                    var authorToRemoveFrom = await this.libHubDbContext.Authors.FindAsync((authors[i]).Id);

                    if (authorToRemoveFrom != null)
                    {
                        authorToRemoveFrom.BookDescriptions.Remove(bookDescriptionToRemove);
                        await this.libHubDbContext.SaveChangesAsync();
                    }

                    return bookDescriptionToRemove;
                }
            }

            return bookDescriptionToRemove;
        }
    }
}
