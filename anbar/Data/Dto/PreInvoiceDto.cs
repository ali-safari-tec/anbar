namespace anbar.Data.Dto
{
    public class PreInvoiceDto
    {
        public int PreInvoiceId { get; set; }
        public int ChangeId { get; set; }
        public string PreInvoiceIdS { get; set; }
        public string PreInvoiceBuyer { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal InvoiceTotalCost { get; set; }
        public int PreDateDay { get; set; }
        public int PreDateMonth { get; set; }
        public int PreDateYear { get; set; }
        public int PreDateDayS { get; set; }
        public int PreDateMonthS { get; set; }
        public int PreDateYearS { get; set; }
    }
}
