using DevTestCOVID19.Configurations;
using DevTestCOVID19.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevTestCOVID19.Utilities
{
    public class StatisticsAPIRequests
    {
        private readonly AppSettings _appSettings;

        public StatisticsAPIRequests(IConfiguration configuration)
        {
            _appSettings = new AppSettings(configuration);
        } 

        public async Task<List<T>> GetAPIResponse<T>(string url)
        {
            List<T> result = new List<T>();

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", _appSettings.XRapidAPIKey },
                    { "x-rapidapi-host", _appSettings.XRapidAPIHost },
                },
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var res = JsonSerializer.Deserialize<APIResponse<T>>(body);
                    result = res.Data;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong!");
            }

            return result;
        }
    }
}
