using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModel
{
    public class UpdateUserViewModel
    {
        public int UserId { get; set; }
        [Required]
        [DisplayName("Tên Khách Hàng")]
        public string Name { get; set; }
        [DisplayName("Mật Khẩu")]
        [StringLength(15,MinimumLength = 5, ErrorMessage = "Password must be from 5 to 15 characters")]
        public string Password { get; set; }
        [StringLength(15, MinimumLength = 5, ErrorMessage = "New Password must be from 5 to 15 characters")]
        [DisplayName("Mật Khẩu Mới")]
        public string NewPassword { get; set; }
        [DisplayName("Nhập Lại Mật Khẩu Mới")]
        public string RetypeNewPassword { get; set; }
        public string Email { get; set; }
        [DisplayName("Địa Chỉ")]
        public string Address { get; set; }
        [DisplayName("Số Điện Thoại")]
        public string PhoneNumber { get; set; }
    }
}