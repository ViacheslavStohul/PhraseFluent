using DistLearning.DataAccess.Enums;

namespace DistLearning.Service.DTO.Responses;

public class BaseCardResponse
{
    public Guid Uuid { get; set; }
    
    public required string Question { get; set; }
    
    public QuestionType QuestionType { get; set; }
    
    public virtual ICollection<BaseAnswerOptionResponse>? AnswerOptions { get; set; }
}