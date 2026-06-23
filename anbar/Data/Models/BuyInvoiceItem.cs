using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace anbar.Data.Models
{
    public class BuyInvoiceItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuyInvoiceItemId { get; set; }
        public int ProductId { get; set; }
        public int BuyInvoiceId { get; set; }
        [ForeignKey("BuyInvoiceId")]
        public BuyInvoice BuyInvoice { get; set; }
        [Required]
        public string ProductCode { get; set; } //my user wants string id for his product
        [Required]
        public string ProductName { get; set; } //name of product
        [Required]
        public decimal InvoiceItemPrice { get; set; }
        [Required]
        public decimal InvoiceFinalPrice { get; set; } = 0;
        public string InvoiceItemPriceS { get; set; }
        public string InvoiceFinalPriceS { get; set; }
        [Required]
        public int InvoiceItemNumber { get; set; }
    }
}
