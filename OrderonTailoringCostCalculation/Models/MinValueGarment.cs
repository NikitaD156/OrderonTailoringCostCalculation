using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("MinValueGarment")]
    public class MinValueGarment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int MultiplierID { get; set; }
        public string Name { get; set; }
        public int RatioFirst { get; set; }
        public int RatioSecond { get; set; }
        public int RatioThird { get; set; }
        public int RatioFourth { get; set; }
    }
}
