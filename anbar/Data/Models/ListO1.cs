using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Models
{
    public class ListO1
    {
        [Key]
        public int ListO1Id { get; set; }
        public decimal O1P11 { get; set; }
        public decimal O1G11 { get; set; }
        public decimal O1P12 { get; set; }
        public decimal O1G12 { get; set; }
        public decimal O1P14 { get; set; }
        public decimal O1G14 { get; set; }
        public decimal O1PM14 { get; set; }
        public decimal O1GM14 { get; set; }
        public decimal O1PM16 { get; set; }
        public decimal O1GM16 { get; set; }
        public decimal O1PM18 { get; set; }
        public decimal O1GM18 { get; set; }
        public decimal O1PO18 { get; set; }
        public decimal O1GO18 { get; set; }
        public decimal O1PO19 { get; set; }
        public decimal O1GO19 { get; set; }
        public int O1day { get; set; }
        public int O1month { get; set; }
        public int O1year { get; set; }
    }
}
