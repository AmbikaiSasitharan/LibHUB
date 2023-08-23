using LibHub.API.Data;
using LibHub.API.Entities;
using LibHub.API.Repository.Contracts;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace LibHub.API.Repository
{
    public class RatingRepository : IRatingRespository
    {
        private readonly LibHubDbContext libHubDbContext;
        public RatingRepository(LibHubDbContext libHubDbContext)
        {
            this.libHubDbContext = libHubDbContext;
        }
        private async Task<bool> RatingExist(int bookDescriptionId, int userId)
        {
            return await this.libHubDbContext.Ratings.AnyAsync(c => c.BookDescriptionId == bookDescriptionId
                                                                    && c.UserId == userId);
        }

        public async Task<Rating> AddRating(RatingToAddDTO ratingToAddDTO, User user, BookDescription bookDescription)
        {
            var rating = await (from searchBookDescriptions in this.libHubDbContext.BookDescriptions
                                where searchBookDescriptions.Id == ratingToAddDTO.BookDescriptionId
                                select new Rating
                                {
                                    User = user,
                                    UserId = user.Id,
                                    BookDescription = bookDescription,
                                    BookDescriptionId = bookDescription.Id,
                                    rating = ratingToAddDTO.rating,
                                    Comment = ratingToAddDTO.Comment,
                                    EntryDate = ratingToAddDTO.EntryDate
                                }).SingleOrDefaultAsync();
            if (await RatingExist(ratingToAddDTO.BookDescriptionId, ratingToAddDTO.UserId) == false) {
                if (rating != null)
                {
                    var result = await this.libHubDbContext.Ratings.AddAsync(rating);
                    await this.libHubDbContext.SaveChangesAsync();
                    return result.Entity;
                }

                return rating;
            }
            return rating;
        }

        public async Task<Rating> GetRating(int Id)
        {
            var rating = await libHubDbContext.Ratings
                                                   .Include(x => x.User)
                                                   .Include(x => x.BookDescription)
                                                   .FirstOrDefaultAsync( i => i.Id == Id);
            return rating;
        }

        public async Task<IEnumerable<Rating>> GetRatingsForBookDescription(int bookDescriptionId)
        {
            var ratings = await this.libHubDbContext.Ratings
                                                    .Include(x => x.BookDescription)
                                                    .Include(x => x.User)
                                                    .Where(i => i.BookDescriptionId == bookDescriptionId)
                                                    .ToListAsync();
            return ratings;
        }

        public async Task<IEnumerable<Rating>> RemoveAllRatingsOfBookDescription(int bookDescriptionId)
        {
            var ratings = await this.libHubDbContext.Ratings
                                                    .Include(x => x.BookDescription)
                                                    .Include(x => x.User)
                                                    .Where(i => i.BookDescriptionId == bookDescriptionId)
                                                    .ToListAsync();

            if (ratings != null)
            {
                foreach (Rating rating in ratings)
                {
                    var ratingToRemove = await this.libHubDbContext.Ratings.FindAsync(rating.Id);

                    if (ratingToRemove != null)
                    {
                        this.libHubDbContext.Ratings.Remove(ratingToRemove);
                        await this.libHubDbContext.SaveChangesAsync();
                    }

                    return (ratings);
                }
            }

            return ratings;
        }

        public async Task<IEnumerable<Rating>> RemovwAllRatingsOfAUser(int userId)
        {
            var ratingsToRemove = await this.libHubDbContext.Ratings
                                                    .Include(x => x.User)
                                                    .Include(x => x.BookDescription)
                                                    .ThenInclude(x => x.Ratings)
                                                    .Where(i => i.UserId == userId)
                                                    .ToListAsync();
            if (ratingsToRemove != null)
            {
                foreach (Rating rating in ratingsToRemove)
                {
                    this.libHubDbContext.Ratings.Remove(rating);
                    await this.libHubDbContext.SaveChangesAsync();
                }
            }

            return ratingsToRemove;
        }

        public async Task<Rating> RemoveRating(int Id)
        {
            var ratingToRemove = await this.libHubDbContext.Ratings.FindAsync(Id);

            if (ratingToRemove != null)
            {
                this.libHubDbContext.Ratings.Remove(ratingToRemove);
                await this.libHubDbContext.SaveChangesAsync();
            }

            return ratingToRemove;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsOfAUser(int userId)
        {
            var ratings = await this.libHubDbContext.Ratings
                                                    .Include(x => x.BookDescription)
                                                    .Include(x => x.User)
                                                    .Where(i => i.UserId == userId)
                                                    .ToListAsync();
            return ratings;
        }
    }
}
