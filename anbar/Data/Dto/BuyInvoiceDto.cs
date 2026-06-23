using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anbar.Data.Dto
{
    public class BuyInvoiceDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceIdS { get; set; }
        public string InvoiceSeller { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal InvoiceTotalCost { get; set; }
        public decimal InvoicePay { get; set; }
        public int DateDay { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
    }
}
