using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class AuthorToAddDTO
    {
        [Required(ErrorMessage ="Please enter a first name.")]
        public string FName { get; set; }
        public string MName { get; set; } = "";
        [Required(ErrorMessage = "Please enter a last name.")]
        public string LName { get; set; }
        public string FullName { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now;
    }
}
