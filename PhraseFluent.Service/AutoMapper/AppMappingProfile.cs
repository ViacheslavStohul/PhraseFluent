using AutoMapper;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Helpers;
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
        CreateMap<TestSearcHelper, TestSearchResponse>();
    }
}