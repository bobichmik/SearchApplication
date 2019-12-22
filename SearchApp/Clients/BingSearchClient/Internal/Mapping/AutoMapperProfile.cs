using AutoMapper;
using Domain.Core.Models;
using BingSearchClient.Internal.Models;

namespace BingSearchClient.Internal.Mapping
{
    /// <summary>
    /// Profile for map Bing response model and core model
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Value, ResponseModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(source => source.Url));
        }
    }
}