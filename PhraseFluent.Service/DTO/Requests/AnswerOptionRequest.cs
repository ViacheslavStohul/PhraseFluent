namespace PhraseFluent.Service.DTO.Requests;

public class AnswerOptionRequest
{
    public required string OptionText { get; set; }

    public required bool IsCorrect;
}