using Azure;
using Azure.AI.Translation.Text;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        
        var otherTranslations = await _client.LookupDictionaryEntriesAsync(translatedTextItem.DetectedLanguage.Language, targetLanguage, wordToTranslate);

        var translationResultResponse = new TranslationResult
        {
            Translation = new TranslatedWord
            {
                Text = translationResult.Text,
                Transliteration = translationResult.Transliteration?.Text
            },
            TranslatedFrom = translatedTextItem.DetectedLanguage.Language
        };

        if (!otherTranslations.HasValue) return translationResultResponse;

        var dictionaryItems = (from dictionaryLookupItem in otherTranslations.Value
            from translation in dictionaryLookupItem.Translations
            select new OtherTranslation
            {
                Text = translation.NormalizedTarget,
                Prefix = translation.PrefixWord,
                Confidence = translation.Confidence
            });

        translationResultResponse.OtherTranslations = dictionaryItems;

        return translationResultResponse;
    }

    public async Task<IEnumerable<UsageExamplesResponse>> GetExamples(string word, string translatedWord, string languageFrom, string languageTo)
    {
        var textWithTranslation = new InputTextWithTranslation(word, translatedWord);
        
        var response = await _client.LookupDictionaryExamplesAsync(languageFrom, languageTo, textWithTranslation);
        ArgumentNullException.ThrowIfNull(response);

        var exampleResponse = new List<UsageExamplesResponse>();

        foreach (var dictionaryExampleItem in response.Value)
        {
            if (dictionaryExampleItem == null) continue;

            exampleResponse.AddRange(dictionaryExampleItem.Examples.Select(example => new UsageExamplesResponse
            {
                TranslatedExample = new UsageExample
                {
                    Prefix = example.TargetPrefix,
                    Term = example.TargetTerm,
                    Suffix = example.TargetSuffix
                },
                NativeLanguageExample = new UsageExample
                {
                    Prefix = example.SourcePrefix,
                    Term = example.SourceTerm,
                    Suffix = example.SourceSuffix
                }
            }));
        }

        return exampleResponse;
    }
}