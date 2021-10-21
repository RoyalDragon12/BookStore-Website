using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class PublisherBookCountViewModel
    {
        public Publisher Publisher { get; set; }
        public int bookCount { get; set; }
    }
}