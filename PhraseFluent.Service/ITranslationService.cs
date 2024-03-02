using Azure;
using Azure.AI.Translation.Text;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service;

public interface ITranslationService
{
    Task<IEnumerable<SupportedLanguage>> GetLanguages();

    Task<TranslationResult> GetWordTranslation(string wordToTranslate, string targetLanguage);

    Task<IEnumerable<UsageExamplesResponse>> GetExamples(string word, string translatedWord, string languageFrom, string languageTo);
}