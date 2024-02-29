namespace PhraseFluent.Service.DTO.Responses;

public class TranslatedWord
{
    public required string Translation { get; set; }
    
    public required string TranslatedFrom { get; set; }
    
    public required string TranslatedTo { get; set; }
}