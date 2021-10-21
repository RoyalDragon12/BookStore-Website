using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage ="A Name is required")]
        [DisplayName("Họ Tên Người Dùng")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A UserName is required")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "User Name must have 5 to 10 characters")]
        [Remote("CheckUserName", "Home", ErrorMessage = "This username has been taken")] //Combine with the ActionResult in HomeController to check duplicated name in real time
        [DisplayName("Tên Tài Khoản")]
        public string UserName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Password must have 5 to 15 characters")]
        [DisplayName("Mật Khẩu")]
        public string Password { get; set; }
        public string Email { get; set; }
        [DisplayName("Địa Chỉ")]
        public string Address { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phonenumber must be numeric")]
        [DisplayName("Số Điện Thoại")]
        public string PhoneNumber { get; set; }
        public string Roles { get; set; } //Customer, Moderator, Admin
        [DisplayName("Trạng Thái Tài Khoản")]
        public int isBanned { get; set; }
        public bool isBannedBool
        {
            get { return isBanned == 1; }
            set { isBanned = value ? 1 : 0; }
        }

        public virtual ICollection<Cart> Carts { get; set; }
    }
}