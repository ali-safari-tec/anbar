using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Models
{
    public class Cost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Seller {  get; set; }
        public string Caption {  get; set; }
        public string SellerPhone {  get; set; }
        public string SellerMobile {  get; set; }
        public string SellerPhoneCode {  get; set; }
        public string SellerAddress {  get; set; }
        [Required]
        public decimal Fee {  get; set; }
        [Required]
        public decimal Pay {  get; set; }
        public decimal Discount {  get; set; }
        public int Type { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string Detail {  get; set; }

    }
}
