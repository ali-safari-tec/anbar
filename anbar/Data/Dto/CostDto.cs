using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Dto
{
    public class CostDto
    {
        public int Id { get; set; }
        public string Seller { get; set; }
        public string Caption { get; set; }
        public decimal Fee { get; set; }
        public decimal Pay { get; set; }
        public decimal Discount { get; set; }
        public int Type { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}
