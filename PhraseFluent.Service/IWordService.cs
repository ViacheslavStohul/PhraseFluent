using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service;

public interface IWordService
{
    /// <summary>
    /// Retrieves the translation of a given word to the specified language.
    /// </summary>
    /// <param name="wordToTranslate">The word to be translated.</param>
    /// <param name="translateTo">The language to translate the word to.</param>
    /// <returns>
    /// An instance of the <see cref="TranslatedWord"/> class containing the translation information.
    /// </returns>
    /// <remarks>
    /// This method internally uses an instance of the <see cref="Translator"/> class to perform the translation.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when either <paramref name="wordToTranslate"/> or <paramref name="translateTo"/> is null or empty.
    /// </exception>
    /// <exception cref="TranslationException">
    /// Thrown when the translation for the given word and language cannot be retrieved.
    /// </exception>
    Task<TranslatedWord> GetWordTranslation(string wordToTranslate, string translateTo);
}