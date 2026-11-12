namespace  EstateFlow.Domain
{
    public class Provider
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Note { get; set; }

        public int UtilityTypeId { get; set; }
        public UtilityType? UtilityType { get; set; }
    }
}
