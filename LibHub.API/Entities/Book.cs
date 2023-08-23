using System;
using System.Collections.Generic;
using System.Text;

namespace LibHub.API.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public BookDescription BookDescription { get; set; }
        public int BookDescriptionId { get; set; }
        public float CostAtTimeOfPurchase { get; set; }
        public string Status { get; set; }
        public string Language { get; set; }
        public ICollection<Borrow> Users { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
