using Azure;
using Azure.AI.Translation.Text;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service;

public interface ITranslationService
{
    Task<Response<GetLanguagesResult>> GetLanguages();
}