using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BorrowToAddDTO
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
