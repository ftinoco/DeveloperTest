using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevTestCOVID19.Models.DTOs
{
    public class APIRegionDTO: APIRegionInfoDTO
    { 
        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("long")]
        public string Long { get; set; }

        [JsonPropertyName("cities")]
        public List<APICityDTO> Cities { get; set; }
    }
}
