using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class AuthorBookCountViewModel
    {
        public Author Author { get; set; }
        public int bookCount { get; set; }
    }
}