using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("MaterialGroup")]
    public class MaterialGroup
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Description { get; set; }
    }
}
