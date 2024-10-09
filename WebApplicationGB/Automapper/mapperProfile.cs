using AutoMapper;
using WebApplicationGB.DTO;
using WebApplicationGB.Models;

namespace WebApplicationGB.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            // используем маппинг. Название свойств entity совпадает с названиями свойств в DTO 
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductGroup, ProductGroupDTO>().ReverseMap();
            // ReverseMap для маппирования в обе стороны
        }
    }
}
