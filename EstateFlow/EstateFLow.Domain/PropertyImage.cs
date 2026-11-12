namespace  EstateFlow.Domain
{
    public class PropertyImage
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public Guid PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
