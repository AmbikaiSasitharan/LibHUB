using System;
using System.Collections.Generic;
using System.Text;

namespace LibHub.API.Entities
{
    public class User
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
        public List<Borrow> Borrows { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public int NumBorrowingBooks { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime EntryDate { get; set; }
    }
}
