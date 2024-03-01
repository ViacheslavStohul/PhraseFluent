using Azure;
using Azure.AI.Translation.Text;
using Microsoft.Extensions.Options;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Options;

namespace PhraseFluent.Service;

public class TranslationService : ITranslationService
{
    private readonly TextTranslationClient _client;
    
    public TranslationService(IOptions<MicrosoftTranslatorSettings> settings)
    {
        AzureKeyCredential key = new(settings.Value.Key);

        _client = new TextTranslationClient(key);
    }
    
    public async Task<Response<GetLanguagesResult>> GetLanguages()
    {
        var languages = await _client.GetLanguagesAsync();

        return languages;
    }
}