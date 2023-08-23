using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BookDescriptionDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public List<string> Genres { get; set; }
        public DateTime PublishDate { get; set; }
        public double Rating { get; set; }
        public int NumCopies { get; set; }
        public int NumAvailable { get; set; }
        public string AllAuthorsInOneString { get; set; }
        public List<int> BookIds { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
