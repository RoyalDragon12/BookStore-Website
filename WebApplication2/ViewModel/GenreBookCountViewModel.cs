using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class GenreBookCountViewModel
    {
        //This model is used in GenreManager, it displays the book count of a genre.
        public Genre Genre { get; set; }
        public int bookCount { get; set; }
    }
}