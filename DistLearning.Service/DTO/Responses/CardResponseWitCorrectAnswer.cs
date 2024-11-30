using DistLearning.DataAccess.Enums;

namespace DistLearning.Service.DTO.Responses;

public class CardResponseWitCorrectAnswer : BaseCardResponse
{
    public new ICollection<AnswerOptionResponseWitCorrectAnswer>? AnswerOptions { get; set; }
}