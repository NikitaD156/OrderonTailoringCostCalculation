using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("ComplicatedElement")]
    public class ComplicatedElement
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int SubgroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Merit { get; set; }

    }
}
