using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class BookDescriptionToAddDTO
    {
        [Required(ErrorMessage = "Please enter a Title.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a Publish.")]
        public DateTime PublishDate { get; set; }
        [Required(ErrorMessage = "Please enter a Description.")]
        public string Description { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Please select at least 1 author.")]
        public List<int> AuthorIds { get; set; }
        
        [Required ]
        [MinLength(1, ErrorMessage = "Please select at least 1 author.")]
        public List<int> GenreIds { get;  set; }
    }
}
