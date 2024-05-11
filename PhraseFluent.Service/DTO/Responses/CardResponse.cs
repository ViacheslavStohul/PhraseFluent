using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.Service.DTO.Responses;

public class CardResponse
{
    public Guid Uuid { get; set; }
    
    public required string Question { get; set; }
    
    public QuestionType QuestionType { get; set; }
    
    public ICollection<AnswerOptionResponse>? AnswerOptions { get; set; }
}