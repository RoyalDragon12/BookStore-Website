using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        [DisplayName("Tác Giả")]
        public string AuthorName { get; set; }
        public int isHidden { get; set; }
        [DisplayName("Chế Độ")]
        public bool isHiddenBool
        {
            get { return isHidden == 1; }
            set { isHidden = value ? 1 : 0; }
        }
    }
}