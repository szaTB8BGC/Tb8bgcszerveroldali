using System.Text.Json.Serialization;

namespace  EstateFlow.Domain
{
    public class UtilityType
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [JsonIgnore]
        public ICollection<Provider> Providers { get; set; } = new List<Provider>();
    }
}
