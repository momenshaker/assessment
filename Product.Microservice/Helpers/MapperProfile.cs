using AutoMapper;
using Product.Microservice.Dto;
using Product.Microservice.Infrastructure.Entities;

namespace Product.Microservice.Helpers
{
    public class MapperProfile : Profile
    {

        /// <summary>
        /// To Map Dto and models 
        /// </summary>
        public MapperProfile()
        {
            CreateMap<Catalog, CatalogDto>()
                     .ForMember(dest => dest.id, opt => opt.MapFrom(x => x.Id))
                     .ForMember(dest => dest.name, opt => opt.MapFrom(x => x.Name))
                     .ForMember(dest => dest.price, opt => opt.MapFrom(x => x.Price))
                     .ForMember(dest => dest.cost, opt => opt.MapFrom(x => x.Cost))
                     .ForMember(dest => dest.imageBase, opt => opt.MapFrom(x => x.ImageBase))
                        .ReverseMap();


        }
    }
}
