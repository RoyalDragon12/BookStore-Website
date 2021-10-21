using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        [DisplayName("Tên Thể Loại")]
        public string GenreName { get; set; }
        [DisplayName("Mã Thể Loại")]
        public string GenreCode { get; set; }
        public int isHidden { get; set; }
        [DisplayName("Chế Độ")]
        public bool isHiddenBool
        {
            get { return isHidden == 1; }
            set { isHidden = value ? 1 : 0; }
        }

        public virtual ICollection<Book_Genre_Junction> Book_Genre_Junctions { get; set; }
    }
}