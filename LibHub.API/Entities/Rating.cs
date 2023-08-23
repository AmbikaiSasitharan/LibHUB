using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.API.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public BookDescription BookDescription  { get; set; }
        public int BookDescriptionId { get; set; }
        public float rating { get; set; }
        public string Comment { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
