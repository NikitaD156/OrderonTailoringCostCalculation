using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("Discount")]
    public class Discount
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int MultiplierID { get; set; }
        public string Name { get; set; }
        public int Merit { get; set; }
    }
}
