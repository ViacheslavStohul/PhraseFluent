namespace DistLearning.Service.DTO.Requests;

public class AnswerOptionRequest
{
    public required string OptionText { get; set; }
    
    public bool IsAllowedText { get; set; }
}