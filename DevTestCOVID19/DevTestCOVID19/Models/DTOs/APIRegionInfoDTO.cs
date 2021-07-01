using System.Text.Json.Serialization;

namespace DevTestCOVID19.Models.DTOs
{
    public class APIRegionInfoDTO
    {
        [JsonPropertyName("iso")]
        public string ISO { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
