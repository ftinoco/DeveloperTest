using AutoMapper;
using DevTestCOVID19.Models;
using DevTestCOVID19.Models.ViewModels;
using DevTestCOVID19.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DevTestCOVID19.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IStatisticsAPIRequests _statisticsAPIRequests;

        public HomeController(IStatisticsAPIRequests statisticsAPIRequests, 
            ILogger<HomeController> logger,  IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _statisticsAPIRequests = statisticsAPIRequests;
        }

        public IActionResult Index()
        { 
            return View();
        }
        
        public async Task<IActionResult> LoadGrid()
        {
            var result = await _statisticsAPIRequests.GetTop10RegionsWithMostCases(DateTime.Now.AddDays(-1));
            var model = _mapper.Map<List<RegionViewModel>>(result);
            return PartialView("Partials/_loadRegionGrid", model);
        }

        public async Task<IActionResult> LoadGridByRegion(string region)
        {
            var model2 = await _statisticsAPIRequests.GetTop10ProvincesWithMostCases(DateTime.Now.AddDays(-1), region);
            var model = _mapper.Map< List<ProvinceViewModel>>(model2);
            return PartialView("Partials/_loadProvinceGrid", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
