namespace  EstateFlow.Domain
{
    public class Contract
    {
        public Guid Id { get; set; }
        public DateTime SignedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? DocumentPath { get; set; }

        public Guid PropertyId { get; set; }
        public Property? Property { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
