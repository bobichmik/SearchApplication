using AutoMapper;
using Domain.Core.Models;
using System.Xml;
using YandexSearchClient.Internal.Models;

namespace YandexSearchClient.Internal.Mapping
{
    /// <summary>
    /// Profile for map yandex response model and core model
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Group, SearchResultModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => CreateName(source.Doc.Title)))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(source => source.Doc.Url));
        }

        private string CreateName(object title)
        {
            string name = null;
            var titleNodes = (XmlNode[])title;
            foreach (var node in titleNodes)
            {
                name += node.InnerText;
            }

            return name;
        }
    }
}
