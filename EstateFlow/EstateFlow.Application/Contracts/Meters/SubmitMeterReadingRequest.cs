namespace EstateFlow.Application.Contracts.Meters
{
    public class SubmitMeterReadingRequest
    {
        public decimal Value { get; set; }
        public DateTime MeasuredAt { get; set; }
    }
}
