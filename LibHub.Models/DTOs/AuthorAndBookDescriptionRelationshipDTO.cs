using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class AuthorAndBookDescriptionRelationshipDTO
    {
        public int AuthorId { get; set; }
        public int BookDescriptionId { get; set; }
    }
}
