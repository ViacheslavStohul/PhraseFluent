namespace PhraseFluent.Service.DTO.Responses;

public class TranslationResult
{
    public TranslatedWord? Translation { get; set; }
    
    public IEnumerable<OtherTranslation>? OtherTranslations { get; set; }
    
    public string? TranslatedFrom { get; set; }
}