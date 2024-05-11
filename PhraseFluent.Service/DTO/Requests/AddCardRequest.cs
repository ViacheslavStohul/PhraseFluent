using PhraseFluent.DataAccess.Enums;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.DTO.Requests;

public class AddCardRequest
{
    public required string Question { get; set; }
    
    public QuestionType QuestionType { get; set; }
    
    public Guid TestUuid { get; set; }
    
    public ICollection<AnswerOptionRequest>? AnswerOptions { get; set; }
}