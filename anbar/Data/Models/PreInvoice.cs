using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace anbar.Data.Models
{
    public class PreInvoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PreInvoiceId { get; set; }
        public int ChangeId { get; set; }
        public string PreInvoiceIdS { get; set; }
        [Required]
        public string PreInvoiceSeller { get; set; }
        [Required]
        public string PreInvoiceBuyer { get; set; }
        public string PreInvoiceAddress { get; set; }
        public string PreInvoicePhone { get; set; }
        public string PreInvoicePhoneCode { get; set; }
        [Required]
        public int PreDateDay { get; set; }
        [Required]
        public int PreDateMonth { get; set; }
        [Required]
        public int PreDateYear { get; set; }
        [Required]
        public int PreDateDayS { get; set; }
        [Required]
        public int PreDateMonthS { get; set; }
        [Required]
        public int PreDateYearS { get; set; }
        [Required]
        public decimal InvoiceTotalCost { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public string Detail { get; set; }
        public string Detail2 { get; set; }
        public virtual ICollection<PreInvoiceItem> preInvoiceItems { get; set; } = new List<PreInvoiceItem>();
    }
}
