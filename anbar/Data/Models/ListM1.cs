using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Models
{
    public class ListM1
    {
        [Key]
        public int ListM1Id { get; set; } 
        public decimal M1P11 { get; set; } 
        public decimal M1G11 { get; set; } 
        public decimal M1P12 { get; set; } 
        public decimal M1G12 { get; set; } 
        public decimal M1P14 { get; set; } 
        public decimal M1G14 { get; set; } 
        public decimal M1PM14 { get; set; } 
        public decimal M1GM14 { get; set; } 
        public decimal M1PM16 { get; set; } 
        public decimal M1GM16 { get; set; } 
        public decimal M1PM18 { get; set; } 
        public decimal M1GM18 { get; set; }
        public decimal M1PO18 { get; set; } 
        public decimal M1GO18 { get; set; } 
        public decimal M1PO19 { get; set; } 
        public decimal M1GO19 { get; set; } 
        public int M1day { get; set; } 
        public int M1month { get; set; }
        public int M1year { get; set; }
    }
}
