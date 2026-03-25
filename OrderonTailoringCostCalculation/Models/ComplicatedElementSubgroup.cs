using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace OrderonTailoringCostCalculation.Models
{
    [Table("ComplicatedElementSubgroup")]
    public class ComplicatedElementSubgroup
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
