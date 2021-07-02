using AutoMapper;
using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Models.ViewModels;
using DevTestCOVID19.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTestCOVID19.BusinessLogic.Implemantations
{
    public class ExportHelperByRegion : ExportHelper, IExportByRegion
    {
        private readonly IMapper _mapper;
        private readonly IAPIRequestService _APIRequestService;

        public ExportHelperByRegion(IAPIRequestService APIRequestService, IMapper mapper)
        {
            _mapper = mapper;
            _APIRequestService = APIRequestService;
        }

        public async Task<byte[]> GetArrayByteToExportCSV()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases();
            var model = _mapper.Map<List<RegionViewModel>>(result);
            List<object> list = (from item in model
                                 select new[] {
                                            item.ISO,
                                            item.Name,
                                            item.Cases.ToString(),
                                            item.Deaths.ToString()
                                      }).ToList<object>();

            list.Insert(0, new string[4] { "ISO", "Name", "Cases", "Deaths" });

            return GetArrayByteToExportCSV(list);
        }

        public async Task<byte[]> GetArrayByteToExportXML()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases();
            var model = _mapper.Map<List<RegionViewModel>>(result);
            return GetArryByteToExportXML("CasesByRegion", "Region", model);
        }
    }
}
