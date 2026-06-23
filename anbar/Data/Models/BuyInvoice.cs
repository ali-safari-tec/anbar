using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace anbar.Data.Models
{
    public class BuyInvoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuyInvoiceId { get; set; }
        public int ChangeId { get; set; }
        public string BuyInvoiceIdS { get; set; }
        [Required]
        public string BuyInvoiceSeller { get; set; }
        public string BuyInvoiceAddress { get; set; }
        public string BuyInvoicePhone { get; set; }
        public string BuyInvoicePhoneCode { get; set; }
        [Required]
        public int DateDay { get; set; }
        [Required]
        public int DateMonth { get; set; }
        [Required]
        public int DateYear { get; set; }
        [Required]
        public decimal BuyInvoiceTotalCost { get; set; }
        public decimal BuyInvoiceDiscount { get; set; }
        public decimal BuyInvoicePay { get; set; }
        [Required]
        public int Type { get; set; }
        public string Detail { get; set; }
        public virtual ICollection<BuyInvoiceItem> BuyInvoiceItems { get; set; } = new List<BuyInvoiceItem>();
    }
}
