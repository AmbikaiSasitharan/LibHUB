using LibHub.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibHub.API.Data
{
    public class LibHubDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookDescription> BookDescriptions { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Renewal> Renewals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public LibHubDbContext(DbContextOptions<LibHubDbContext> options)
            : base(options)
        {

        }
    }
}
