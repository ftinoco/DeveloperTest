using DevTestCOVID19.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevTestCOVID19.BusinessLogic.Interfaces
{
    public interface IAPIRequestService
    {
        Task<List<APIRegionInfoDTO>> GetAllRegions();
        Task<List<APIResponseDTO>> GetTop10RegionsWithMostCases(DateTime date);
        Task<List<APIResponseDTO>> GetTop10ProvincesWithMostCases(DateTime date, string ISO); 
    }
}
