using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace anbar.Data.Models
{
    public class InvoiceItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceItemId { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
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
