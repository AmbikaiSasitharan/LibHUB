using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BorrowDetailsDTO
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public int BookDescriptionId { get; set; }  
        public string BookTitle { get; set; }
        public int BookId { get; set; }
        public int NumOfRevewals { get; set; }
        public float Cost { get; set; }
        public int NumRenewals { get; set; }
        public DateTime EntryDate { get; set; }
        public bool AreFeesFined { get; set; }
        public int NumberOfDaysTillFeesAreApplied { get; set; }
        public bool IsLateNotified { get; set; }
        public bool IsFineNotified { get; set; }
    }
}
