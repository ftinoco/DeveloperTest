using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Configurations;
using DevTestCOVID19.Models.DTOs;
using DevTestCOVID19.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTestCOVID19.BusinessLogic.Implemantations
{
    public class APIRequestService : IAPIRequestService
    {
        private readonly AppSettings _appSettings;
        private readonly StatisticsAPIRequests _statisticsAPIRequests;

        public APIRequestService(IConfiguration configuration)
        {
            _appSettings = new AppSettings(configuration);
            _statisticsAPIRequests = new StatisticsAPIRequests(configuration);
        }

        public async Task<List<APIRegionInfoDTO>> GetAllRegions()
        {
            var result = await _statisticsAPIRequests.GetAPIResponse<APIRegionInfoDTO>($"{_appSettings.BaseAPIURL}/regions");
            return result.OrderBy(x => x.Name).ToList();
        }
         
        public async Task<List<APIResponseDTO>> GetTop10ProvincesWithMostCases(string ISO)
        {
            var result = await _statisticsAPIRequests.GetAPIResponse<APIResponseDTO>($"{_appSettings.BaseAPIURL}/reports?iso={ISO}");
            return result.OrderByDescending(x => x.Confirmed).Take(10).ToList();
        }

        public async Task<List<APIResponseDTO>> GetTop10RegionsWithMostCases()
        { 
            var result = await _statisticsAPIRequests.GetAPIResponse<APIResponseDTO>($"{_appSettings.BaseAPIURL}/reports");
            List<APIResponseDTO> lst = new List<APIResponseDTO>();
            // Grouping the result by region
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

    }
}
