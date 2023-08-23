using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BookDetailsDTO
    {
        public int Id { get; set; }
        public int BookDescriptionId { get; set; }
        public string Status { get; set; }
        public string Language { get; set; }
        public DateTime EntryDate { get; set; }
        public float CostAtTimeOfPurchase { get; set; }

    }
}
