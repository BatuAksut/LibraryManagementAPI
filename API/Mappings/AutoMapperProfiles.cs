using API.Models;
using API.Models.Dtos;
using AutoMapper;

namespace API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<AddCategoryDto,Category>().ReverseMap();
            CreateMap<UpdateCategoryDto,Category>().ReverseMap();
            CreateMap<AddBookDto, Book>().ReverseMap();
            CreateMap<UpdateBookDto,Book>().ReverseMap(); 

        }
    }
}
