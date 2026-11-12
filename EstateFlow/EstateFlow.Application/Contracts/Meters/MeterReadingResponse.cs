namespace EstateFlow.Application.Contracts.Meters
{
    public class MeterReadingResponse
    {
        public Guid Id { get; set; }
        public DateTime MeasuredAt { get; set; }
        public decimal Value { get; set; }
        public DateTime ReportedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? PhotoPath { get; set; }
        public Guid MeterId { get; set; }
    }
}
