using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BookDescriptionInventoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public DateTime PublishDate { get; set; }
        public float Rating { get; set; }
        public float NumRatings { get; set; }
        public int NumAvailable { get; set; }
        public bool IsActive { get; set; }
        public string AllAuthorsInOneString { get; set; }

    }
}
