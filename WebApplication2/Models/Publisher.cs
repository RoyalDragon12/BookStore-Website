using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }

        [DisplayName("Tên Nhà Xuất Bản")]
        public string PublisherName { get; set; }
        [DisplayName("Mã Nhà Xuất Bản")]
        public string PublisherCode { get; set; }
        [DisplayName("Địa Chỉ")]
        public string Address { get; set; }
        [DisplayName("Số Điện Thoại")]
        public string PhoneNumber { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int isHidden { get; set; }
        [DisplayName("Chế Độ")]
        public bool isHiddenBool
        {
            get { return isHidden == 1; }
            set { isHidden = value ? 1 : 0; }
        }
    }
}