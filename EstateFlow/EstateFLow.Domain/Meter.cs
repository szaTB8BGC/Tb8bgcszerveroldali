namespace  EstateFlow.Domain
{
    public class Meter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required string SerialNumber { get; set; }
        public string? ReadingPeriod { get; set; }
        public string? Note { get; set; }

        public Guid PropertyId { get; set; }
        public Property? Property { get; set; }

        public int UtilityTypeId { get; set; }
        public UtilityType? UtilityType { get; set; }

        public int ProviderId { get; set; }
        public Provider? Provider { get; set; }

        public ICollection<MeterReading> Readings { get; set; } = new List<MeterReading>();
    }
}
