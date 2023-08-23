using System;
using System.Collections.Generic;
using System.Text;

namespace LibHub.API.Entities
{
    public class Renewal
    {
        //Entity Attribute: Id, Borrow, BorrowId, OrginalDueDate
        //ChangedDueDate, EntryDate
        public int Id { get; set; }
        public Borrow Borrow { get; set; }
        public int BorrowId { get; set; }
        public DateTime OriginalDueDate { get; set; }
        public DateTime ChangedDueDate { get; set; }
        public DateTime EntryDate { get; set; }

    }
}
