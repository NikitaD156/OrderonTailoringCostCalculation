using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("Receipt")]
    public class Receipt
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int MinValueGarmentID { get; set; }
        public int MaterialGroupID { get; set; }
        public double GroupCoefficient { get; set; }
        public int ConventionalUnitValue { get; set; }
        public int GarmentRatio { get; set; }
        public double MinValue { get; set; }
        public double ComplicatedElementsValue { get; set; }
        public double DiscountValue { get; set; }
        public double Extras { get; set; }
        public double TotalValue { get; set; }

    }
}
