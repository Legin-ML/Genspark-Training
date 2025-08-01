using AutoMapper;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Models.DTOs;

namespace ShopMigrationAPI.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Color, ColorDTO>().ReverseMap();
        CreateMap<News, NewsDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}