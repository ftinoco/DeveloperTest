using DevTestCOVID19.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DevTestCOVID19.Configurations;
using DevTestCOVID19.Models;

namespace DevTestCOVID19.Utilities
{
    public class StatisticsAPIRequests : IStatisticsAPIRequests
    {
        private readonly AppSettings _appSettings;

        public StatisticsAPIRequests(IConfiguration configuration)
        {
            _appSettings = new AppSettings(configuration);
        }

        public async Task<List<APIRegionInfoDTO>> GetAllRegions()
        { 
            var result = await GetAPIResponse<APIRegionInfoDTO>($"{_appSettings.BaseAPIURL}/regions");
            return result.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<APIResponseDTO>> GetTop10ProvincesWithMostCases(DateTime date, string ISO)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            var result = await GetAPIResponse<APIResponseDTO>($"{_appSettings.BaseAPIURL}/reports?date={formattedDate}&iso={ISO}");
            return result.OrderByDescending(x => x.Confirmed).Take(10).ToList();
        }

        public async Task<List<APIResponseDTO>> GetTop10RegionsWithMostCases(DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            var result = await GetAPIResponse<APIResponseDTO>($"{_appSettings.BaseAPIURL}/reports?date={formattedDate}");
            List<APIResponseDTO> lst = new List<APIResponseDTO>();
            result.ForEach(x =>
            {
                var item = lst.Where(y => x.Region.Name.Equals(y.Region.Name)).FirstOrDefault();
                if (item == null)
                {
                    x.Region.Cities = null;
                    x.Region.Province = null;
                    lst.Add(x);
                }
                else
                {
                    item.Confirmed += x.Confirmed;
                    item.ActiveDiff += x.ActiveDiff;
                    item.Active += x.Active;
                    item.Recovered += x.Recovered;
                    item.ConfirmedDiff += x.ConfirmedDiff;
                    item.DeathsDiff += x.DeathsDiff;
                    item.RecoveredDiff += x.RecoveredDiff;
                    item.Deaths += x.Deaths;
                    item.FatalityRate = (item.Deaths / (decimal)item.Confirmed) * 100;
                }
            });
            return lst.OrderByDescending(x => x.Confirmed).Take(10).ToList();
        }

        private async Task<List<T>> GetAPIResponse<T>(string url)
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
