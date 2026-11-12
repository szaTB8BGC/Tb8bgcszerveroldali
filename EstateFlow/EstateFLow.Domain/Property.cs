namespace  EstateFlow.Domain
{
    public class Property
    {
        public Guid Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int SquareMeters { get; set; }
        public string? AdvertisementText { get; set; }

        public int CityId { get; set; }
        public City? City { get; set; }

        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public ICollection<Meter> Meters { get; set; } = new List<Meter>();
    }
}
