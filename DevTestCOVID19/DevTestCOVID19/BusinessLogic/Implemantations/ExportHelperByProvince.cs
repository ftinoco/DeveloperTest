using AutoMapper;
using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Models.ViewModels;
using DevTestCOVID19.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTestCOVID19.BusinessLogic.Implemantations
{
    public class ExportHelperByProvince : ExportHelper, IExportByProvince
    {
        private readonly IMapper _mapper;
        private readonly IAPIRequestService _APIRequestService;
        public ExportHelperByProvince(IAPIRequestService APIRequestService, IMapper mapper)
        {
            _mapper = mapper;
            _APIRequestService = APIRequestService;
        }

        public async Task<byte[]> GetArrayByteToExportCSV(string ISO)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(ISO);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            List<object> list = (from item in model
                                 select new[] {
                                            item.Name,
                                            item.Cases.ToString(),
                                            item.Deaths.ToString()
                                      }).ToList<object>();

            list.Insert(0, new string[3] { "Name", "Cases", "Deaths" });

            return GetArrayByteToExportCSV(list);
        }

        public async Task<byte[]> GetArrayByteToExportXML(string ISO)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(ISO);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            return this.GetArryByteToExportXML("CasesByProvince", "Province", model);
        }
    }
}
