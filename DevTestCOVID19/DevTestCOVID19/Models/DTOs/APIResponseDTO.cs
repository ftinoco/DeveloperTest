using System;
using System.Text.Json.Serialization;

namespace DevTestCOVID19.Models.DTOs
{
    public class APIResponseDTO
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("confirmed")]
        public long Confirmed { get; set; }

        [JsonPropertyName("deaths")]
        public long Deaths { get; set; }

        [JsonPropertyName("recovered")]
        public long Recovered { get; set; }

        [JsonPropertyName("confirmed_diff")]
        public long ConfirmedDiff { get; set; }

        [JsonPropertyName("deaths_diff")]
        public long DeathsDiff { get; set; }

        [JsonPropertyName("recovered_diff")]
        public long RecoveredDiff { get; set; }

        [JsonPropertyName("last_update")]
        public string LastUpdate { get; set; }

        [JsonPropertyName("active")]
        public long Active { get; set; }

        [JsonPropertyName("active_diff")]
        public long ActiveDiff { get; set; }

        [JsonPropertyName("fatality_rate")]
        public decimal FatalityRate { get; set; }

        [JsonPropertyName("region")]
        public APIRegionDTO Region { get; set; }
    }
}
