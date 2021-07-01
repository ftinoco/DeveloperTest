using System;
using System.Text.Json.Serialization;

namespace DevTestCOVID19.Models.DTOs
{
    public class APICityDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("fips")]
        public long? Fips { get; set; }

        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("long")]
        public string Long { get; set; }

        [JsonPropertyName("confirmed")]
        public long Confirmed { get; set; }

        [JsonPropertyName("deaths")]
        public long Deaths { get; set; }

        [JsonPropertyName("confirmed_diff")]
        public long ConfirmedDiff { get; set; }

        [JsonPropertyName("deaths_diff")]
        public long DeathsDiff { get; set; }

        [JsonPropertyName("last_update")]
        public string LastUpdate { get; set; }
    }
}
