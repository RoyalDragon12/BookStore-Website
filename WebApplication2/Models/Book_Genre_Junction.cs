using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Book_Genre_Junction
    {
        //this is when a book has multiple genres. Will be implemented later.
        [Key, Column(Order = 1)]
        public int BookId { get; set; }

        [Key, Column(Order = 2)]
        public int GenreId { get; set; }
    }
}