using AutoMapper;
using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Models;
using DevTestCOVID19.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevTestCOVID19.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IExportByRegion _exportByRegion;
        private readonly IExportByProvince _exportByProvince;
        private readonly IAPIRequestService _APIRequestService;

        private readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public HomeController(IMapper mapper,
            IExportByRegion exportByRegion,
            IExportByProvince exportByProvince,
            IAPIRequestService APIRequestService)
        {
            _mapper = mapper;
            _exportByRegion = exportByRegion;
            _exportByProvince = exportByProvince;
            _APIRequestService = APIRequestService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region By Region

        public async Task<IActionResult> LoadGridByRegion()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases();
            var model = _mapper.Map<List<RegionViewModel>>(result);
            return PartialView("Partials/_loadRegionGrid", model);
        }

        [HttpPost]
        public async Task<IActionResult> ExportCSVByRegion()
        {
            return File(await _exportByRegion.GetArrayByteToExportCSV(), "text/csv", "CasesByRegion.csv");
        }

        [HttpPost]
        public async Task<IActionResult> ExportJSONByRegion()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases();
            var model = _mapper.Map<List<RegionViewModel>>(result);
            return File(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model, options)), "application/json", "CasesByRegion.json");
        }

        [HttpPost]
        public async Task<IActionResult> ExportXMLByRegion()
        {
            return File(await _exportByRegion.GetArrayByteToExportXML(), "application/xml", "CasesByRegion.xml");
        }

        #endregion

        #region By Province

        public async Task<IActionResult> LoadGridByProvince(string region)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(region);
            if (result == null)
                return NotFound();
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            return PartialView("Partials/_loadProvinceGrid", model);
        }

        [HttpPost]
        public async Task<IActionResult> ExportCSVByProvince(string region)
        {
            return File(await _exportByProvince.GetArrayByteToExportCSV(region), "text/csv", "CasesByProvince.csv");
        }

        [HttpPost]
        public async Task<IActionResult> ExportJSONByProvince(string region)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(region);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            return File(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model, options)), "application/json", "CasesByProvince.json");
        }

        [HttpPost]
        public async Task<IActionResult> ExportXMLByProvince(string region)
        {
            return File(await _exportByProvince.GetArrayByteToExportXML(region), "application/xml", "CasesByProvince.xml");
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}