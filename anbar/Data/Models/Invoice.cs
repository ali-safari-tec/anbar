using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace anbar.Data.Models
{
    public class Invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }
        public int ChangeId { get; set; }
        public string InvoiceIdS { get; set; }
        [Required]
        public string InvoiceSeller { get; set; }
        [Required]
        public string InvoiceBuyer { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoicePhone { get; set; }
        public string InvoicePhoneCode { get; set; }
        [Required]
        public int DateDay { get; set; }
        [Required]
        public int DateMonth { get; set; }
        [Required]
        public int DateYear { get; set; }
        [Required]
        public decimal InvoiceTotalCost { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal InvoicePay { get; set; }
        [Required]
        public int Type { get; set; }
        public string Detail { get; set; }
        public string Detail2 { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}
