using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("Multiplier")]
    public class Multiplier
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double FirstGroup { get; set; }
        public double SecondGroup { get; set; }
        public double ThirdGroup { get; set; }
        public double FourthGroup { get; set; }
    }
}
