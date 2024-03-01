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
        var translatorSettings = settings.Value;
        
        AzureKeyCredential key = new(translatorSettings.Key);

        _client = new TextTranslationClient(key, translatorSettings.Region);
    }
    
    public async Task<IEnumerable<SupportedLanguage>> GetLanguages()
    {
        var response = await _client.GetLanguagesAsync();
        
        ArgumentNullException.ThrowIfNull(response);

        var supportedLanguages = response.Value.Translation.Select(language => new SupportedLanguage
        {
            Name = language.Value.Name,
            NativeName = language.Value.NativeName,
            Key = language.Key
        });

        return supportedLanguages;
    }

    public async Task<TranslationResult> GetWordTranslation(string wordToTranslate, string targetLanguage)
    {
        var response = await _client.TranslateAsync(new[] { targetLanguage }, new[] { wordToTranslate }, toScript: "Latn");
        ArgumentNullException.ThrowIfNull(response);
        
        var translatedTextItem = response.Value[0];
        ArgumentNullException.ThrowIfNull(translatedTextItem);

        var translationResult = translatedTextItem.Translations[0];

        var translationResultResponse = new TranslationResult
        {
            Translation = new TranslatedWord
            {
                Text = translationResult.Text,
                Transliteration = translationResult.Transliteration.Text
            },
            TranslatedFrom = translatedTextItem.DetectedLanguage.Language
        };

        return translationResultResponse;
    }
}