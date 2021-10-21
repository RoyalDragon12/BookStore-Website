using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        [DisplayName("Chi Tiết Thông Báo")]
        public string NotiDescription { get; set; }
        [DisplayName("Tình Trạng")]
        public int NotiState { get; set; } //0 = unread, 1 = read
        [DisplayName("Ngày Nhận Tin")]
        public DateTime NotiDate { get; set; }
        public int UserId { get; set; }
        [DisplayName("Mã Đơn Hàng")]
        public int CartId { get; set; }
        public virtual User Users { get; set; }
    }
}