using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    public partial class CartInfo
    {
        public int CartInfoId { get; set; }
        public int BookId { get; set; }
        public int CartId { get; set; }

        [Range(1,99, ErrorMessage = "Lượng đặt phải lớn hơn 0 và nhỏ hơn 100")]
        [Remote("CheckAvailableAmount", "Home",AdditionalFields ="BookId", ErrorMessage = "Không đủ sách trong kho")] //Combine with the ActionResult in HomeController to check available amount to buy
        [DisplayName("Số Lượng")]
        public int Amount { get; set; }
        [DisplayName("Giá Khách Mua")]
        public float SingleCost { get; set; }
        [DisplayName("Tổng Tiền")]
        public float TotalCost { get; set; }

        public virtual Cart Carts { get; set; }
        public virtual Book Books { get; set; }
    }
}