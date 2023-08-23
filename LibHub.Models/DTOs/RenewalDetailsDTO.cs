using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class RenewalDetailsDTO
    {
        public int Id { get; set; }
        public int BorrowId { get; set; }
        public DateTime OriginalDueDate { get; set; }
        public DateTime ChangedDueDate { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
