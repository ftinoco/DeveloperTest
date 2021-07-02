using AutoMapper;
using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Models.ViewModels;
using DevTestCOVID19.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTestCOVID19.ViewComponents
{
    public class RegionViewComponent : ViewComponent
    {
        private readonly IMapper _mapper; 
        private readonly IAPIRequestService _APIRequestService;

        public RegionViewComponent(IMapper mapper,
            IAPIRequestService statisticsAPIRequests)
        {
            _mapper = mapper; 
            _APIRequestService = statisticsAPIRequests;
        }
        public async Task<IViewComponentResult> InvokeAsync(string selectedValue)
        {
            ViewData["SelectedValue"] = selectedValue;
            var result = await _APIRequestService.GetAllRegions();
            var model = _mapper.Map<List<RegionInfoViewModel>>(result);
            return View("Default", model);
        }
    }
}
