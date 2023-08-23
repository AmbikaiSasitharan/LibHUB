using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class UserWithLateBorrowsDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<BorrowDetailsDTO> borrows { get; set; }
        public int NumBorrowsLate { get; set; }
        public float TotalAmountOwed { get; set; }

    }
}
