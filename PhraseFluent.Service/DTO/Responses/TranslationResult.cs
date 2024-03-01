namespace PhraseFluent.Service.DTO.Responses;

public class TranslationResult
{
    public TranslatedWord? Translation { get; set; }
    
    public string? TranslatedFrom { get; set; }
}