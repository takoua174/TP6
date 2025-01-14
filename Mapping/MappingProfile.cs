using AutoMapper;
using TP6.DTO;
using TP6.Models;

namespace TP6.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map Category -> CategoryDTO
        CreateMap<Category, CategoryDTO>();

        // Map CategoryDTO -> Category
        CreateMap<CategoryDTO, Category>();
    }
}
