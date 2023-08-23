using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace LibHub.API.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly LibHubDbContext libHubDbContext;

        public GenreRepository(LibHubDbContext libHubDbContext)
        {
            this.libHubDbContext = libHubDbContext;
        }
        public async Task<Genre> AddBookDescriptionToGenre(int Id, int bookDescriptionId)
        {
            var genreToAddTo = await this.libHubDbContext.Genres.FindAsync(Id);
            var bookDescriptionToAdd = await this.libHubDbContext.BookDescriptions.FindAsync(bookDescriptionId);

            if ((genreToAddTo != null) || (bookDescriptionToAdd != null))
            {
                genreToAddTo.BookDescriptions.Add(bookDescriptionToAdd);
                await this.libHubDbContext.SaveChangesAsync();
                return genreToAddTo;
            }

            return null;
        }

        public async Task<Genre> AddGenre(GenreToAddDTO genreToAdd)
        {
            var existingAuthor = await this.libHubDbContext.Genres.FirstOrDefaultAsync(u => u.Name == genreToAdd.Name);

            if (existingAuthor == null)
            {

                var genre = new Genre
                {
                    Name = genreToAdd.Name
                };

                var result = await this.libHubDbContext.Genres.AddAsync(genre);
                await this.libHubDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            var genres = await this.libHubDbContext.Genres
                                                   .ToListAsync();
            return genres;
        }

        public async Task<Genre> GetGenre(int Id)
        {
            var genre = await this.libHubDbContext.Genres
                                                   .Include(x => x.BookDescriptions)
                                                   .FirstOrDefaultAsync(i => i.Id == Id);
            return genre;
        }

        public async Task<BookDescription> RemoveBookDescriptionFromGenres(List<Genre> genres, int bookDescriptionId)
        {
            var bookDescriptionToRemove = await this.libHubDbContext.BookDescriptions.FindAsync(bookDescriptionId);
            if (bookDescriptionToRemove != null)
            {
                for (var i = 0; i < genres.Count; i++)
                {
                    var genreToRemoveFrom = await this.libHubDbContext.Genres.FindAsync((genres[i]).Id);

                    if (genreToRemoveFrom != null)
                    {
                        genreToRemoveFrom.BookDescriptions.Remove(bookDescriptionToRemove);
                        await this.libHubDbContext.SaveChangesAsync();
                    }

                    return bookDescriptionToRemove;
                }
            }

            return null;
        }

        public async Task<Genre> RemoveGenre(int Id)
        {
            var genreToRemove = await this.libHubDbContext.Genres.FindAsync(Id);

            if (genreToRemove != null)
            {
                this.libHubDbContext.Genres.Remove(genreToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return genreToRemove;
        }
    }
}
