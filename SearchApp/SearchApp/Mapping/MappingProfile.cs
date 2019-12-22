using AutoMapper;
using SearchApp.Models;
using Domain.Core.Models;

namespace SearchApp.Mapping
{
    /// <summary>
    /// Profile for map Db model and Core model
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ResultModel, ResponseModel>().ReverseMap();
        }
    }
}
