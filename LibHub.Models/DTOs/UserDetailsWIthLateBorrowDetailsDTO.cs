using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class UserDetailsWIthLateBorrowDetailsDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }
        public DateTime BirthDate { get; set; }
        public int NumBorrowingBooks { get; set; }
        public bool IsActive { get; set; }
        public DateTime EntryDate { get; set; }
        public bool HasLateBorrow { get; set; }
    }
}
