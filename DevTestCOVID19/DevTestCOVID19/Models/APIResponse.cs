using DevTestCOVID19.Models.DTOs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevTestCOVID19.Models
{
    public class APIResponseError
    {
        public string Message { get; set; }
    }

    public class APIResponse <TData>
    {
        [JsonPropertyName("data")]
        public List<TData> Data { get; set; }
    }
}
