using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string PromoName { get; set; }
        public string PromoDesc { get; set; }
        public DateTime PromoStartDate { get; set; }
        public DateTime PromoEndDate { get; set; }
        public int PromoState { get; set; } //Applicable = 1, Expired = 0
        public virtual ICollection<PromotionDetail> PromotionDetails { get; set; }
    }
}