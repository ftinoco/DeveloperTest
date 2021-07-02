using AutoMapper;
using DevTestCOVID19.BusinessLogic.Interfaces;
using DevTestCOVID19.Controllers;
using DevTestCOVID19.Models.DTOs;
using DevTestCOVID19.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevTestCOVID19Test
{
    public class HomeControllerTest
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly HomeController controller;
        private readonly Mock<IAPIRequestService> mockService;
        private readonly Mock<IExportByRegion> mockExportByRegion;
        private readonly Mock<IExportByProvince> mockExportByProvince;

        public HomeControllerTest()
        {
            mockService = new Mock<IAPIRequestService>();
            mockExportByRegion = new Mock<IExportByRegion>();
            mockExportByProvince = new Mock<IExportByProvince>();
            mockMapper = new Mock<IMapper>();

            controller = new HomeController(mockMapper.Object, mockExportByRegion.Object,
                mockExportByProvince.Object, mockService.Object);
        }

        [Fact]
        public async Task LoadGridByRegionTest()
        {
            // Arrange
            var mockAPIRequest = new List<APIResponseDTO>
            {
                new APIResponseDTO {
                    Active = 3762,
                    ActiveDiff = 0,
                    Confirmed = 8178,
                    ConfirmedDiff = 0,
                    Date = DateTime.Now.ToLongDateString(),
                    Deaths = 191,
                    DeathsDiff = 0,
                    FatalityRate = 0.0234M,
                    LastUpdate = DateTime.Now.AddDays(-1).ToLongDateString(),
                    Recovered = 4225,
                    RecoveredDiff = 0,
                    Region = new APIRegionDTO()
                    {
                        ISO = "NIC",
                        Name= "Nicaragua",
                        Province = string.Empty,
                        Lat= "12.8654",
                        Long= "-85.2072",
                        Cities = new List<APICityDTO>()
                    }
                },
            };
            mockService.Setup(repo => repo.GetTop10RegionsWithMostCases()).Returns(Task.FromResult(mockAPIRequest));

            var lst = new List<RegionViewModel>
            {
                  new RegionViewModel {
                 Cases = 8178,
                 Deaths= 191,
                 ISO = "NIC",
                 Name    = "Nicaragua"
                },
            };
            mockMapper.Setup(x => x.Map<List<RegionViewModel>>(mockAPIRequest)).Returns(lst);

            // Act
            var result = await controller.LoadGridByRegion();

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<RegionViewModel>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task LoadGridByProvinceTest()
        {
            // Arrange
            var mockAPIRequest = new List<APIResponseDTO>();
            mockService.Setup(repo => repo.GetTop10ProvincesWithMostCases("USA")).Returns(Task.FromResult(mockAPIRequest));

            var lst = new List<ProvinceViewModel>
            {
                  new ProvinceViewModel {
                     Cases = 3817211,
                     Deaths= 63706,
                     Name    = "California"
                    },
                  new ProvinceViewModel {
                     Cases = 3001682,
                     Deaths= 52410,
                     Name    = "Texa"
                    },
                  new ProvinceViewModel {
                     Cases = 2365464,
                     Deaths= 37772,
                     Name    = "Florida"
                    },
                  new ProvinceViewModel {
                     Cases = 2115377,
                     Deaths= 53690,
                     Name    = "New York"
                    },
                  new ProvinceViewModel {
                     Cases = 1392196,
                     Deaths= 25670,
                     Name    = "Illinois"
                    },
            };
            mockMapper.Setup(x => x.Map<List<ProvinceViewModel>>(mockAPIRequest)).Returns(lst);

            // Act
            var result = await controller.LoadGridByProvince("USA");

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProvinceViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(5, model.Count());
        }

        [Fact]
        public async Task LoadGridByProvinceTest_ReturnsNotFound_WhenNoISOProvided()
        {
            // Arrange
            var mockAPIRequest = new List<APIResponseDTO>();
            mockService.Setup(repo => repo.GetTop10ProvincesWithMostCases(string.Empty)).Returns(Task.FromResult(mockAPIRequest));

            // Act
            var result = await controller.LoadGridByProvince(null);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

    }
}
