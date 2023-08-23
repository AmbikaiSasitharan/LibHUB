using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class AuthorDetailsDTO
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FullName { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
