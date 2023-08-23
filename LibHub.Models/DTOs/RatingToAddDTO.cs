using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class RatingToAddDTO
    {
        public int UserId { get; set; }
        public int BookDescriptionId { get; set; }
        [Required(ErrorMessage = "Please enter a rating.")]
        [Range(0, 5,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public float rating { get; set; }

        [Required(ErrorMessage = "Please enter a comment.")]
        public string Comment { get; set; } = "";
        public DateTime EntryDate { get; set; } = DateTime.Now;
    }
}
