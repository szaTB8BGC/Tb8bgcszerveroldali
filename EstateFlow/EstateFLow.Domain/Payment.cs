namespace  EstateFlow.Domain
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTime PaidAt { get; set; }
        public decimal RentAmount { get; set; }
        public decimal UtilitiesAmount { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public string? Note { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public Guid PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
