using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class GenreToAddDTO
    {
        [Required(ErrorMessage = "Please enter a genre name.")]
        public string Name {  get; set; }
    }
}
