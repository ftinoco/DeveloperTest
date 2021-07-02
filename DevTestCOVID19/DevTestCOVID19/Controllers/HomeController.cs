using AutoMapper;
using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Models;
using DevTestCOVID19.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace DevTestCOVID19.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIRequestService _APIRequestService;

        public HomeController(IAPIRequestService APIRequestService,
            ILogger<HomeController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _APIRequestService = APIRequestService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region By Region

        public async Task<IActionResult> LoadGridByRegion()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases(DateTime.Now.AddDays(-1));
            var model = _mapper.Map<List<RegionViewModel>>(result);
            return PartialView("Partials/_loadRegionGrid", model);
        }

        [HttpPost]
        public async Task<IActionResult> ExportCSVByRegion()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases(DateTime.Now.AddDays(-1));
            var model = _mapper.Map<List<RegionViewModel>>(result);
            List<object> list = (from item in model
                                 select new[] {
                                            item.ISO,
                                            item.Name,
                                            item.Cases.ToString(),
                                            item.Deaths.ToString()
                                      }).ToList<object>();

            list.Insert(0, new string[4] { "ISO", "Name", "Cases", "Deaths" });

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string[] customer = (string[])list[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    sb.Append(customer[j] + ',');
                }
                sb.Append("\r\n");
            }

            return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "COVID19CasesByRegion.csv");
        }

        [HttpPost]
        public async Task<IActionResult> ExportJSONByRegion()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases(DateTime.Now.AddDays(-1));
            var model = _mapper.Map<List<RegionViewModel>>(result);
            return File(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)), "application/json", "COVID19CasesByRegion.json");
        }

        [HttpPost]
        public async Task<IActionResult> ExportXMLByRegion()
        {
            var result = await _APIRequestService.GetTop10RegionsWithMostCases(DateTime.Now.AddDays(-1));
            var model = _mapper.Map<List<RegionViewModel>>(result);
            using (MemoryStream stream = new MemoryStream())
            { 
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.ASCII);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;

                xmlWriter.WriteStartDocument(); 
                xmlWriter.WriteStartElement("CasesByRegion");

                foreach (var item in model)
                { 
                    xmlWriter.WriteStartElement("Region");
                     
                    xmlWriter.WriteElementString("ISO", item.ISO);
                    xmlWriter.WriteElementString("Name", item.Name);
                    xmlWriter.WriteElementString("Cases", item.Cases.ToString());
                    xmlWriter.WriteElementString("Deaths", item.Deaths.ToString());
                     
                    xmlWriter.WriteEndElement();
                }
                 
                xmlWriter.WriteEndElement(); 
                xmlWriter.WriteEndDocument(); 
                xmlWriter.Flush(); 
                byte[] byteArray = stream.ToArray();                 
                xmlWriter.Close();

                return File(byteArray, "application/xml", "COVID19CasesByRegion.xml");
            }
        }

        #endregion

        #region By Province

        public async Task<IActionResult> LoadGridByProvince(string region)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(DateTime.Now.AddDays(-1), region);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            return PartialView("Partials/_loadProvinceGrid", model);
        }

        [HttpPost]
        public async Task<IActionResult> ExportCSVByProvince(string region)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(DateTime.Now.AddDays(-1), region);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            List<object> list = (from item in model
                                 select new[] {
                                            item.Name,
                                            item.Cases.ToString(),
                                            item.Deaths.ToString()
                                      }).ToList<object>();

            list.Insert(0, new string[3] { "Name", "Cases", "Deaths" });

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string[] customer = (string[])list[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    sb.Append(customer[j] + ',');
                }
                sb.Append("\r\n");
            }

            return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "COVID19CasesByProvince.csv");
        }

        [HttpPost]
        public async Task<IActionResult> ExportJSONByProvince(string region)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(DateTime.Now.AddDays(-1), region);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            return File(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)), "application/json", "COVID19CasesByProvince.json");
        }

        [HttpPost]
        public async Task<IActionResult> ExportXMLByProvince(string region)
        {
            var result = await _APIRequestService.GetTop10ProvincesWithMostCases(DateTime.Now.AddDays(-1), region);
            var model = _mapper.Map<List<ProvinceViewModel>>(result);
            using (MemoryStream stream = new MemoryStream())
            { 
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.ASCII);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;

                xmlWriter.WriteStartDocument(); 
                xmlWriter.WriteStartElement("CasesByProvince");

                foreach (var item in model)
                { 
                    xmlWriter.WriteStartElement("Province"); 

                    xmlWriter.WriteElementString("Name", item.Name);
                    xmlWriter.WriteElementString("Cases", item.Cases.ToString());
                    xmlWriter.WriteElementString("Deaths", item.Deaths.ToString()); 

                    xmlWriter.WriteEndElement();
                }
                 
                xmlWriter.WriteEndElement(); 
                xmlWriter.WriteEndDocument(); 
                xmlWriter.Flush(); 
                byte[] byteArray = stream.ToArray();
                xmlWriter.Close();

                return File(byteArray, "application/xml", "COVID19CasesByProvince.xml");
            }
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}