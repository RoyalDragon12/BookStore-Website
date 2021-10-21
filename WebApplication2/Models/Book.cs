using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Book
    {
        public int BookId { get; set; }
        [DisplayName("Tên Sách")]
        [Required]
        public string BookName { get; set; }
        [DisplayName("Mã Sách")]
        public string BookCode { get; set; }
        [DisplayName("Giá Tiền")]
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Giá Tiền Chỉ Chấp Nhận Số")]
        public float Cost { get; set; }
        [DisplayName("Mô Tả")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayName("Hình Ảnh")]
        public string Image { get; set; }
        [DisplayName("Ngày Cập Nhật")]
        public DateTime UpdatedDate { get; set; }
        [DisplayName("Tồn Kho")]
        public int StorageAmount { get; set; }

        public int isHidden { get; set; }
        [DisplayName("Chế Độ")]
        public bool isHiddenBool {
            get { return isHidden == 1; }
            set { isHidden = value ? 1 : 0; }
        }

        [DisplayName("Tên Nhà Phát Hành")]
        public int PublisherId { get; set; }

        [DisplayName("Tác Giả")]
        public int AuthorId { get; set; }

        public virtual ICollection<Book_Genre_Junction> Book_Genre_Junctions { get; set; }
        public virtual Publisher Publishers { get; set; }
        public virtual Author Authors { get; set; }
    }
}