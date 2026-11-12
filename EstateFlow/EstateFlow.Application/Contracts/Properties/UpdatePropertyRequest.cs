namespace EstateFlow.Application.Contracts.Properties
{
    public class UpdatePropertyRequest
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int SquareMeters { get; set; }
        public string? AdvertisementText { get; set; }
        public int CityId { get; set; }
    }
}
