using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Cart
    {
        [DisplayName("Mã Đơn Hàng")]
        public int CartId { get; set; }
        [DisplayName("Mã Khách Hàng")]
        public int UserId { get; set; }
        [DisplayName("Hình Thức Thanh Toán")]
        public int Paid { get; set; } //0 = pay after shipping, 1 = pre-paid
        public bool PaidBool //automatically turn integer Paid into a bool to aid in @Html.CheckBoxFor
        {
            get { return Paid == 1; } 
            set { Paid = value ? 1 : 0; }
        }
        [DisplayName("Tình Trạng Đơn Hàng")]
        public int Completed { get; set; } //0 = ordering. 1 = order completed. Bonus: -1 = order cancelled.
        public int ShipmentProgress { get; set; } //0 = pending, 1 = shipping, 2 = shipped
        [DisplayName("Địa Chỉ Giao Hàng")]
        public string ShippingAddress { get; set; }
        [DisplayName("Tiền Vận Chuyển")]
        public float ShippingCost { get; set; }
        [DisplayName("Tiền Đơn Hàng")]
        public float TotalCost { get; set; }
        [DisplayName("Ngày Đặt Hàng")]
        public DateTime PurchaseDate { get; set; }
        [DisplayName("Ngày Giao Hàng")]
        public DateTime DeliveryDate { get; set; }
        [DisplayName("Chi Tiết Đơn Hàng")]

        public string CartNote { get; set; }

        public virtual ICollection<CartInfo> CartInfos { get; set; }
        public virtual User Users { get; set; }
    }
}