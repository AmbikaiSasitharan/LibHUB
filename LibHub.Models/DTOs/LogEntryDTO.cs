using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class LogEntryDTO
    {
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DueDate { get; set; }
        public string RenewalOrBorrow { get; set; }
        public DateTime OriginalDueDate { get; set; }
        public int NumOfRenewals { get; set; }
        public DateTime DayReturned { get; set; }
        public DateTime ChangedDueDate { get; set; }
    }
}
