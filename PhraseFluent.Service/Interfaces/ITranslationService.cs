using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.Interfaces;

public interface ITranslationService
{
    /// <summary>
    /// Retrieves a list of supported languages for translation.
    /// </summary>
    /// <returns>
    /// An asynchronous task that represents the operation. The task result contains an enumerable of LanguageResponse objects, which represent the supported languages.
    /// </returns>
    Task<IEnumerable<LanguageResponse>> GetLanguages();

    /// <summary>
    /// Retrieves the translation of a word.
    /// </summary>
    /// <param name="wordToTranslate">The word to be translated.</param>
    /// <param name="targetLanguage">The language to which the word should be translated.</param>
    /// <returns>The translation result containing the translated word and other translations (if available).</returns>
    Task<TranslationResult> GetWordTranslation(string wordToTranslate, string targetLanguage);

    /// <summary>
    /// Retrieves usage examples for a specified word and translation.
    /// </summary>
    /// <param name="word">The word to retrieve examples for.</param>
    /// <param name="translatedWord">The translated word to retrieve examples for.</param>
    /// <param name="languageFrom">The source language of the word.</param>
    /// <param name="languageTo">The target language of the target word.</param>
    /// <returns>A collection of usage example responses.</returns>
    Task<IEnumerable<UsageExamplesResponse>> GetExamples(string word, string translatedWord, string languageFrom, string languageTo);
}