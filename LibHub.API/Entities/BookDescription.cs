using System;
using System.Collections.Generic;
using System.Text;

namespace LibHub.API.Entities
{
    //Entity Attributes: Id, Title, PublishDate, Rating, List<Book> Books,
    //                   ICollection<Write> Authors, List<Genre> Genres,Description, EntryDate
    public class BookDescription
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public float Rating { get; set; }
        public float NumRatings { get; set; }
        public int NumCopies { get; set; }
        public int NumAvailable { get; set; }
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }
        public List<Genre> Genres { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime EntryDate { get; set; }
    }
}
