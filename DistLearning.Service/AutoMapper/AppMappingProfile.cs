using AutoMapper;
using DistLearning.DataAccess.Entities;
using DistLearning.DataAccess.Helpers;
using DistLearning.Service.DTO.Responses;

namespace DistLearning.Service.AutoMapper;

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