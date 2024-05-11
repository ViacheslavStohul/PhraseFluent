namespace PhraseFluent.Service.DTO.Responses;

public class AnswerOptionResponse
{
    public Guid Uuid { get; set; }
    
    public required string OptionText { get; set; }

    public required bool IsCorrect;
}