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
        CreateMap<Card, CardResponseWitCorrectAnswer>();
        CreateMap<Card, BaseCardResponse>();
        CreateMap<AnswerOption, AnswerOptionResponseWitCorrectAnswer>();
        CreateMap<AnswerOption, BaseAnswerOptionResponse>();
        CreateMap<PaginationHelper<Test>, PaginationResponse<TestResponse>>();
    }
}