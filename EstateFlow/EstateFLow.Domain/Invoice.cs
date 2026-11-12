namespace  EstateFlow.Domain
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public required string InvoiceNumber { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string? FilePath { get; set; }
        public decimal Amount { get; set; }

        public Guid PropertyId { get; set; }
        public Property? Property { get; set; }

        public int ProviderId { get; set; }
        public Provider? Provider { get; set; }
    }
}
