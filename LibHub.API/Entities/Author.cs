using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibHub.API.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FullName { get; set; }
        public List<BookDescription> BookDescriptions { get; set; }
        public DateTime EntryDate { get; set; }

    }
}
