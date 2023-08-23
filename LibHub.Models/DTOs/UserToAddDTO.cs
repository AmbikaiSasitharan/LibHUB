using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class UserToAddDTO
    {
        [Required(ErrorMessage = "Please enter a username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a first name.")]
        public string FName { get; set; }
        public string MName { get; set; } = "";
        [Required(ErrorMessage = "Please enter a last name.")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Please enter a email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter an address.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter a phone number.")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter a birth date.")]
        public DateTime BirthDate { get; set; }
        public int NumBorrowingBooks { get; set; }
        public DateTime EntryDate { get; set; }
    }
}

