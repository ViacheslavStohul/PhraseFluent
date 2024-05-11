using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.Service.DTO.Responses;

public class CardResponseWitCorrectAnswer : BaseCardResponse
{
    public new ICollection<AnswerOptionResponseWitCorrectAnswer>? AnswerOptions { get; set; }
}