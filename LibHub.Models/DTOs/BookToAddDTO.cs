using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BookToAddDTO
    {
        public int BookDescriptionId { get; set; }

        [Required(ErrorMessage = "Please enter a language.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Please enter a const at time of purchase.")]
        public float CostAtTimeOfPurchase { get; set; }

    }
}
