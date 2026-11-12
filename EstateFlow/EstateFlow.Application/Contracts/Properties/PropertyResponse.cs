namespace EstateFlow.Application.Contracts.Properties
{
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int SquareMeters { get; set; }
        public string? AdvertisementText { get; set; }
        public int CityId { get; set; }
        public required string CityName { get; set; }
    }
}
