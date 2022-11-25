using AutoMapper;
using WebAPI.Models;

namespace WebAPI.Utilities;

public class AuthorsProfile : Profile
{
    public AuthorsProfile()
    {
        CreateMap<Author, AuthorDTO>()
                  .ForMember(
                    dest => dest.Name,
                    options => options.MapFrom(source => $"{source.FirstName} {source.LastName}"))
                  .ForMember(
                    dest => dest.Age,
                    options => options.MapFrom(source => DateTime.Now.Year - source.DateOfBirth.Year));

        CreateMap<AuthorCreationDTO, Author>();
        CreateMap<Author, AuthorFullDTO>();
    }
}
