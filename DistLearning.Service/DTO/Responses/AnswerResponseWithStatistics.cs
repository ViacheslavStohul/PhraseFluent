namespace DistLearning.Service.DTO.Responses;

public class AnswerResponseWithStatistics : BaseAnswerOptionResponse
{
    public int SelectionCount { get; set; }
    public int SelectionPercentage { get; set; }
}