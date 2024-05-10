using AutoMapper;
using Azure;
using Azure.AI.Translation.Text;
using Microsoft.Extensions.Options;
using PhraseFluent.DataAccess.Repositories.Interfaces;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Interfaces;
using PhraseFluent.Service.Options;

namespace PhraseFluent.Service;

public class TranslationService : ITranslationService
{
    private readonly TextTranslationClient _client;
    private readonly ILanguageRepository _languageRepository;
    private readonly IMapper _mapper;
    
    public TranslationService(IOptions<MicrosoftTranslatorSettings> settings, ILanguageRepository languageRepository, IMapper mapper)
    {
        _languageRepository = languageRepository;
        _mapper = mapper;
        var translatorSettings = settings.Value;
        
        AzureKeyCredential key = new(translatorSettings.Key);

        _client = new TextTranslationClient(key, translatorSettings.Region);
    }

    /// <summary>
    /// Retrieves a list of supported languages for translation.
    /// </summary>
    /// <returns>
    /// An asynchronous task that represents the operation. The task result contains an enumerable of LanguageResponse objects, which represent the supported languages.
    /// </returns>
    public async Task<IEnumerable<LanguageResponse>> GetLanguages()
    {
        var languages = await _languageRepository.GetAll();
        
        var responses = languages.Select(_mapper.Map<LanguageResponse>).ToList();

        return responses;
    }

    /// <summary>
    /// Retrieves the translation of a word.
    /// </summary>
    /// <param name="wordToTranslate">The word to be translated.</param>
    /// <param name="targetLanguage">The language to which the word should be translated.</param>
    /// <returns>The translation result containing the translated word and other translations (if available).</returns>
    public async Task<TranslationResult> GetWordTranslation(string wordToTranslate, string targetLanguage)
    {
        var response = await _client.TranslateAsync(new[] { targetLanguage }, new[] { wordToTranslate }, toScript: "Latn");
        ArgumentNullException.ThrowIfNull(response);
        
        var translatedTextItem = response.Value[0];
        ArgumentNullException.ThrowIfNull(translatedTextItem);

        var translationResult = translatedTextItem.Translations[0];
        
        var otherTranslations = await _client.LookupDictionaryEntriesAsync(translatedTextItem.DetectedLanguage.Language, targetLanguage.ToLower(), wordToTranslate);

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

    /// <summary>
    /// Retrieves usage examples for a specified word and translation.
    /// </summary>
    /// <param name="word">The word to retrieve examples for.</param>
    /// <param name="translatedWord">The translated word to retrieve examples for.</param>
    /// <param name="languageFrom">The source language of the word.</param>
    /// <param name="languageTo">The target language of the target word.</param>
    /// <returns>A collection of usage example responses.</returns>
    public async Task<IEnumerable<UsageExamplesResponse>> GetExamples(string word, string translatedWord, string languageFrom, string languageTo)
    {
        var textWithTranslation = new InputTextWithTranslation(word, translatedWord);
        
        var response = await _client.LookupDictionaryExamplesAsync(languageFrom.ToLower(), languageTo.ToLower(), textWithTranslation);
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