using AutoMapper;
using Domain.Core.Models;
using GoogleSearchClient.Internal.Models;

namespace GoogleSearchClient.Internal.Mapping
{
    /// <summary>
    /// Profile for map Google response model and core model
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Item, ResponseModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => source.Title))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(source => source.Link));
        }
    }
}