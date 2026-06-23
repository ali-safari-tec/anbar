namespace anbar.Data.Dto
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public int ChangeId { get; set; }
        public string InvoiceIdS { get; set; }
        public string InvoiceBuyer { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal InvoiceTotalCost { get; set; }
        public decimal InvoicePay { get; set; }
        public int DateDay { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public int Type { get; set; }
    }
}
