using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModel
{
    public class GenreViewModel
    {
        //This model is used when moderators create or edit a book.
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string GenreCode { get; set; }
        
        public bool GenreIsChecked { get; set; }
    }
}