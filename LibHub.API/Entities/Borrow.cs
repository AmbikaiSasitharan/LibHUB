using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibHub.API.Entities
{
    public class Borrow
    {
        //Entity Attribute: Id, DateTime, User, UserId, 
        //                  Book, BookId, IsReturned, 
        //                  NumOfRenewals, Renewals, EntryDate
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public bool IsReturned { get; set; }
        public DateTime DateOfReturn { get; set; }
        public int NumOfRevewals { get; set; }
        public List<Renewal> Renewals { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsLateNotified { get; set; } = false;
        public bool IsFineNotified { get; set; } = false;
    }
}
