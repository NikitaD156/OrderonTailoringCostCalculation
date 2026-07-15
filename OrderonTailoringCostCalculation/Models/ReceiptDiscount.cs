using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("ReceiptDiscount")]
    public class ReceiptDiscount
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ReceiptID { get; set; }
        public int DiscountID { get; set; }
    }
}
