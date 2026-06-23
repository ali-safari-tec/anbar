using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Models
{
    public class ListM
    {
        [Key]
        public int ListMId { get; set; }
        public decimal MP14 { get; set; }
        public decimal MG14 { get; set; }
        public decimal MP16 { get; set; }
        public decimal MG16 { get; set; }
        public decimal MP18 { get; set; }
        public decimal MG18 { get; set; }
        public decimal MP19 { get; set; }
        public decimal MG19 { get; set; }
        public decimal MP20 { get; set; }
        public decimal MG20 { get; set; }
        public decimal MP22 { get; set; }
        public decimal MG22 { get; set; }
        public decimal MP24 { get; set; }
        public decimal MG24 { get; set; }
        public decimal MP27 { get; set; }
        public decimal MG27 { get; set; }
        public decimal MP28 { get; set; }
        public decimal MG28 { get; set; }
        public decimal MP30 { get; set; }
        public decimal MG30 { get; set; }
        public decimal MP32 { get; set; }
        public decimal MG32 { get; set; }
        public int Mday { get; set; }
        public int Mmonth { get; set; }
        public int Myear { get; set; }
    }
}
