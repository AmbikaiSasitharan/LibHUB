using LibHub.API.Entities;
using LibHub.Models.DTOs;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LibHub.API.Extensions
{
    public static class DTOConversions
    {
        public static double CheckIfRatingNull(double val)
        {
            if (double.IsNaN(val)) return 0;
            else return val;
        }
        public static IEnumerable<BookDescriptionInventoryDTO> ConvertToDTO(this IEnumerable<BookDescription> bookDescriptions)
        {
            return (from bookDescription in bookDescriptions
                    select new BookDescriptionInventoryDTO
                    {
                        Id = bookDescription.Id,
                        Title = bookDescription.Title,
                        Authors = bookDescription.Authors.Select(a => a.FullName).ToList(),
                        PublishDate = bookDescription.PublishDate,
                        Rating = bookDescription.Rating,
                        NumRatings = bookDescription.NumRatings,
                        NumAvailable = bookDescription.NumAvailable,
                        IsActive = bookDescription.IsActive,
                        AllAuthorsInOneString = string.Join(",", bookDescription.Authors.Select(a => a.FullName).ToList())
                    }).ToList();
        }

        public static BookDescriptionDetailsDTO ConvertToDTO(this BookDescription bookDescription)
        {
            return new BookDescriptionDetailsDTO
            {
                Id = bookDescription.Id,
                Title = bookDescription.Title,
                Authors = bookDescription.Authors.Select(a => a.FullName).ToList(),
                Genres = bookDescription.Genres.Select(a => a.Name).ToList(),
                PublishDate = bookDescription.PublishDate,
                Rating = bookDescription.Rating,
                NumCopies = bookDescription.NumCopies,
                NumAvailable = bookDescription.NumAvailable,
                AllAuthorsInOneString = string.Join(",", bookDescription.Authors.Select(a => a.FullName).ToList()),
                BookIds = bookDescription.Books.Select(a => a.Id).ToList(),
                IsActive = bookDescription.IsActive,
                Description = bookDescription.Description
            };

        }

        public static IEnumerable<BookDetailsDTO> ConvertToDTO(this IEnumerable<Book> books)
        {
            return (from book in books
                    select new BookDetailsDTO
                    {
                        Id = book.Id,
                        BookDescriptionId = book.BookDescriptionId,
                        Status = book.Status,
                        Language = book.Language,
                        EntryDate = book.EntryDate,
                        CostAtTimeOfPurchase = book.CostAtTimeOfPurchase,
                    }).ToList();
        }

        public static BookDetailsDTO ConvertToDTO(this Book book)
        {
            return new BookDetailsDTO
            {
                Id = book.Id,
                BookDescriptionId = book.BookDescriptionId,
                Status = book.Status,
                Language = book.Language,
                EntryDate = book.EntryDate,
                CostAtTimeOfPurchase = book.CostAtTimeOfPurchase
            };
            
        }

        public static IEnumerable<UserInventoryDTO> ConvertToDTO(this IEnumerable<User> users)
        {
            return (from user in users
                    select new UserInventoryDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FName = user.FName,
                        MName = user.MName,
                        LName = user.LName,
                        Email = user.Email,
                        Address = user.Address,
                        PhoneNum = user.PhoneNum,
                        IsActive = user.IsActive,
                        BirthDate = user.BirthDate,
                        NumBorrowingBooks = user.NumBorrowingBooks
                    }).ToList();
        }

        public static UserDetailsDTO ConvertToDTO(this User user)
        {
            return new UserDetailsDTO
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
                NumBorrowingBooks = user.NumBorrowingBooks,
                IsActive = user.IsActive,
                EntryDate = user.EntryDate
            };

        }

        public static UserDetailsWIthLateBorrowDetailsDTO ConvertToDTOForUsersWithLatesDetails(this User user)
        {
            return new UserDetailsWIthLateBorrowDetailsDTO
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
                NumBorrowingBooks = user.NumBorrowingBooks,
                IsActive = user.IsActive,
                EntryDate = user.EntryDate,
                HasLateBorrow = user.Borrows.Any(b => ((b.DueDate.Date < DateTime.Now.Date) && (b.IsReturned == false)))
            };

        }

        public static UserWithLateBorrowsDTO ConvertToDTO(this User user, IEnumerable<BorrowDetailsDTO> borrows)
        {
            return new UserWithLateBorrowsDTO
                    {
                        UserId = user.Id,
                        FullName = string.Join(" ", new List<string> { user.FName, user.MName, user.LName }),
                        UserName = user.UserName,
                        Email = user.Email,
                        borrows = borrows,
                        NumBorrowsLate = (user.Borrows).Count,
                        TotalAmountOwed = (borrows.Where(bo => (DateTime.Now.Date - bo.DueDate.Date).Days > 7).Select(b => b.Cost)).Sum()
            };
        }

        public static BorrowDetailsDTO ConvertToDTO(this Borrow borrow)
        {
            return new BorrowDetailsDTO
            {
                Id = borrow.Id,
                DueDate = borrow.DueDate,
                BookTitle = borrow.Book.BookDescription.Title,
                BookId = borrow.BookId,
                BookDescriptionId = borrow.Book.BookDescriptionId,
                NumOfRevewals = borrow.NumOfRevewals,
                NumRenewals = (borrow.Renewals.ToList()).Count,
                EntryDate = borrow.EntryDate,
                AreFeesFined = (((DateTime.Now).Date - (borrow.DueDate).Date).Days > 7),
                NumberOfDaysTillFeesAreApplied = ((DateTime.Now).Date - (borrow.DueDate).Date).Days,
                Cost = borrow.Book.CostAtTimeOfPurchase,
                IsLateNotified = borrow.IsLateNotified,
                IsFineNotified = borrow.IsFineNotified
            };

        }

        public static LogEntryDTO ConvertToLogEntryDTO(this Borrow borrow)
        {
            return new LogEntryDTO
            {
                BorrowId = borrow.Id,
                BookId = borrow.BookId,
                BookTitle = borrow.Book.BookDescription.Title,
                EntryDate = borrow.EntryDate,
                DueDate = borrow.EntryDate.AddDays(14),
                RenewalOrBorrow = "Borrow",
                OriginalDueDate = DateTime.Now,
                NumOfRenewals = borrow.NumOfRevewals,
                DayReturned = borrow.DateOfReturn,
                ChangedDueDate = DateTime.Now
            };

        }

        public static LogEntryDTO ConvertToLogEntryDTO(this Renewal renewal, Borrow borrow)
        {
            return new LogEntryDTO
            {
                BorrowId = borrow.Id,
                BookId = borrow.BookId,
                BookTitle = borrow.Book.BookDescription.Title,
                EntryDate = renewal.EntryDate,
                DueDate = borrow.DueDate,
                RenewalOrBorrow = "Renewal",
                OriginalDueDate = renewal.OriginalDueDate,
                NumOfRenewals = borrow.NumOfRevewals,
                DayReturned = borrow.DateOfReturn,
                ChangedDueDate = renewal.ChangedDueDate
            };

        }

        public static IEnumerable<BorrowDetailsDTO> ConvertToDTO(this IEnumerable<Borrow> borrows)
        {
            return (from borrow in borrows
                    select new BorrowDetailsDTO
                    {
                        Id = borrow.Id,
                        DueDate = borrow.DueDate,
                        BookTitle = borrow.Book.BookDescription.Title,
                        BookId = borrow.BookId,
                        BookDescriptionId = borrow.Book.BookDescriptionId,
                        NumOfRevewals = borrow.NumOfRevewals,
                        NumRenewals = (borrow.Renewals.ToList()).Count,
                        EntryDate = borrow.EntryDate,
                        AreFeesFined = ((DateTime.Now).Date - (borrow.DueDate).Date).Days > 7,
                        NumberOfDaysTillFeesAreApplied = ((DateTime.Now).Date - (borrow.DueDate).Date).Days,
                        Cost = borrow.Book.CostAtTimeOfPurchase,
                        IsFineNotified = borrow.IsFineNotified,
                        IsLateNotified = borrow.IsLateNotified

                    }).ToList();
        }

        public static RatingDetailsDTO ConvertToDTO(this Rating rating)
        {
            return new RatingDetailsDTO
            {
                Id = rating.Id,
                UserId = rating.UserId,
                Username = rating.User.UserName,
                BookDescriptionId = rating.BookDescriptionId,
                rating = rating.rating,
                Comment = rating.Comment,
                EntryDate = rating.EntryDate
            };
        }

        public static IEnumerable<RatingDetailsDTO> ConvertToDTO(this IEnumerable<Rating> ratings)
        {
            return (from singleRating in ratings
                    select new RatingDetailsDTO
                    {
                        Id = singleRating.Id, 
                        UserId = singleRating.UserId,
                        Username = singleRating.User.UserName,
                        BookDescriptionId = singleRating.BookDescriptionId,
                        rating = singleRating.rating,
                        Comment = singleRating.Comment,
                        EntryDate = singleRating.EntryDate
                    }).ToList();
        }

        public static IEnumerable<AuthorDetailsDTO> ConvertToDTO(this IEnumerable<Author> authors)
        {
            return (from author in authors
                    select new AuthorDetailsDTO
                    {
                        Id = author.Id,
                        FName = author.FName,
                        MName = author.MName,
                        LName = author.LName,
                        FullName = author.FullName,
                        EntryDate = author.EntryDate,                        
                    }).ToList();
        }

        public static AuthorDetailsDTO ConvertToDTO(this Author author)
        {
            return new AuthorDetailsDTO
            {
                Id = author.Id,
                FName = author.FName,
                MName = author.MName,
                LName = author.LName,
                FullName = author.FullName,
                EntryDate = author.EntryDate,
            };
        }

        public static RenewalDetailsDTO ConvertToDTO(this Renewal renewal)
        {
            return new RenewalDetailsDTO
            {
                Id = renewal.Id,
                BorrowId = renewal.BorrowId,
                OriginalDueDate = renewal.OriginalDueDate,
                ChangedDueDate = renewal.ChangedDueDate,
                EntryDate = renewal.EntryDate
            };
        }

        public static IEnumerable<GenreDetailsDTO> ConvertToDTO(this IEnumerable<Genre> genres)
        {
            return (from genre in genres
                    select new GenreDetailsDTO
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    }).ToList();
        }

        public static GenreDetailsDTO ConvertToDTO(this Genre genre)
        {
            return new GenreDetailsDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public static LibraryStatsDTO ConvertToDTO(this IEnumerable<Book> books, IEnumerable<User> users, IEnumerable<Borrow> borrows)
        {
            return new LibraryStatsDTO
            {
                total_num_user = users.Count(),
                total_num_books = books.Count(),
                total_num_overdue_books_without_fines = (borrows
                                                                .Where(b => ((!b.IsReturned) &&
                                                                             (b.DueDate.Date) < (DateTime.Now.Date)) &&
                                                                             (((DateTime.Now).Date - (b.DueDate).Date).Days <= 7)
                                                                       )
                                                        ).Count(),
                total_num_overdue_books_with_fines = (borrows
                                                                .Where(b => ((!b.IsReturned) &&
                                                                             (b.DueDate.Date) < (DateTime.Now.Date)) &&
                                                                             (((DateTime.Now).Date - (b.DueDate).Date).Days > 7)
                                                                       )
                                                        ).Count(),
                total_num_user_with_overdue_books_without_fines = (users
                                                                        .Where(u => (u.Borrows.Any(b => ((!b.IsReturned) &&
                                                                                                         (b.DueDate.Date) < (DateTime.Now.Date) &&
                                                                                                         (((DateTime.Now).Date - (b.DueDate).Date).Days <= 7)
                                                                                                         )
                                                                                                   )    
                                                                                     )  
                                                                               )
                                                                   ).Count(),
                total_num_user_with_overdue_books_with_fines = (users
                                                                        .Where(u => (u.Borrows.Any(b => ((!b.IsReturned) &&
                                                                                                         (b.DueDate.Date) < (DateTime.Now.Date) &&
                                                                                                         (((DateTime.Now).Date - (b.DueDate).Date).Days > 7)
                                                                                                         )
                                                                                                   )
                                                                                     )
                                                                               )
                                                                   ).Count()

            };
        }
    }
}

