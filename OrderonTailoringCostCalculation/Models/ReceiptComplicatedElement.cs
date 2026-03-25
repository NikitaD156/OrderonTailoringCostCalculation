using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("ReceiptComplicatedElement")]
    public class ReceiptComplicatedElement
    {
        public int ReceiptID { get; set; }
        public int ComplicatedElementID { get; set; }
        public int Count { get; set; }
    }
}
