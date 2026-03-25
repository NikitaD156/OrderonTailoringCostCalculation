using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("ReceiptDiscount")]
    public class ReceiptDiscount
    {
        public int ReceiptID { get; set; }
        public int DiscountID { get; set; }
    }
}
