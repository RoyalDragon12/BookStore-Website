using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication2.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base()
        {
            string database_name = "QuanLyBanSach";
            Database.Connection.ConnectionString = "Data Source=(local);Initial Catalog=" + database_name + ";Trusted_Connection=Yes";
        }

        public DbSet<CartInfo> CartInfos { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> Users {get;set;}
        public DbSet<Publisher> Publishers {get;set;}
        public DbSet<Book> Books {get;set;}
        public DbSet<Author> Authors {get;set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionDetail> PromotionDetails { get; set; }
        public DbSet<Book_Genre_Junction> Book_Genre_Junctions { get; set; }
    }
}