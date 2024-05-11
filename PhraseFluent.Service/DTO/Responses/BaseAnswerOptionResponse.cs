namespace PhraseFluent.Service.DTO.Responses;

public class BaseAnswerOptionResponse
{
    public Guid Uuid { get; set; }
    
    public required string OptionText { get; set; }
}