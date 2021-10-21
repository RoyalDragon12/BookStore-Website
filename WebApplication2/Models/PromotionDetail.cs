using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PromotionDetail
    {
        public int PromotionDetailId { get; set; }
        public float DiscountedCost { get; set; }
        public int BookId { get; set; }
        public int PromotionId { get; set; }
        public virtual Book Books { get; set; }
        public virtual Promotion Promotions { get; set; }
    }
}