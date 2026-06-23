using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Models
{
    public class ListO
    {
        [Key]
        public int ListOId { get; set; }
        public decimal OP14 { get; set; }
        public decimal OG14 { get; set; }
        public decimal OP16 { get; set; }
        public decimal OG16 { get; set; }
        public decimal OP18 { get; set; }
        public decimal OG18 { get; set; }
        public decimal OP19 { get; set; }
        public decimal OG19 { get; set; }
        public decimal OP20 { get; set; }
        public decimal OG20 { get; set; }
        public decimal OP22 { get; set; }
        public decimal OG22 { get; set; }
        public decimal OP24 { get; set; }
        public decimal OG24 { get; set; }
        public decimal OP27 { get; set; }
        public decimal OG27 { get; set; }
        public decimal OP28 { get; set; }
        public decimal OG28 { get; set; }
        public decimal OP30 { get; set; }
        public decimal OG30 { get; set; }
        public decimal OP32 { get; set; }
        public decimal OG32 { get; set; }
        public int Oday { get; set; }
        public int Omonth { get; set; }
        public int Oyear { get; set; }
    }
}
