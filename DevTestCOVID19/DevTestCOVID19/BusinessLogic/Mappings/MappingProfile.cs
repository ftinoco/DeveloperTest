using AutoMapper;
using DevTestCOVID19.Models.DTOs;
using DevTestCOVID19.Models.ViewModels;
using System.Linq;

namespace DevTestCOVID19.BusinessLogic.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<APIResponseDTO, RegionViewModel>()
                .ForMember(x => x.ISO, y => y.MapFrom(src => src.Region.ISO))
                .ForMember(x => x.Name, y => y.MapFrom(src => src.Region.Name))
                .ForMember(x => x.Cases, y => y.MapFrom(src => src.Confirmed))
                .ForMember(x => x.Deaths, y => y.MapFrom(src => src.Deaths));

            CreateMap<APIResponseDTO, ProvinceViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.Region.Province) ? src.Region.Name : src.Region.Province))
                .ForMember(x => x.Cases, y => y.MapFrom(src =>
                    (src.Region.Cities == null || src.Region.Cities.Count > 0) ? src.Region.Cities.Sum(z => z.Confirmed) : src.Confirmed))
                .ForMember(x => x.Deaths, y => y.MapFrom(src =>
                    (src.Region.Cities == null || src.Region.Cities.Count > 0) ? src.Region.Cities.Sum(z => z.Deaths) : src.Deaths));

            CreateMap<APIRegionInfoDTO, RegionInfoViewModel>();
        }
    }
}
