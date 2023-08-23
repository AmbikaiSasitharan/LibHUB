using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibHub.Models.DTOs
{
    public class LibraryStatsDTO
    {
        public int total_num_user {get; set; }
        public int total_num_books { get; set; }
        public int total_num_overdue_books_without_fines { get; set; }
        public int total_num_overdue_books_with_fines { get; set; }
        public int total_num_user_with_overdue_books_without_fines { get; set; }
        public int total_num_user_with_overdue_books_with_fines { get; set; }

    }
}
