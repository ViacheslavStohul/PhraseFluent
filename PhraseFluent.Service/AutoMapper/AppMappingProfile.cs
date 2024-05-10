using AutoMapper;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.AutoMapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<Language, LanguageResponse>();
        CreateMap<Test, TestResponse>();
        CreateMap<Card, CardResponse>();
    }
}