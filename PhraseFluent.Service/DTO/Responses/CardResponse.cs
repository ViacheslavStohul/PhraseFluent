using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.Service.DTO.Responses;

public class CardResponse
{
    public required string Question { get; set; }
    
    public QuestionType QuestionType { get; set; }
}