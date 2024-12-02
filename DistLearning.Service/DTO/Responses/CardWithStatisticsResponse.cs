namespace DistLearning.Service.DTO.Responses;

public class CardWithStatisticsResponse : CardResponse
{
    public new List<AnswerResponseWithStatistics> AnswerOptions { get; set; } = [];
        
    public List<TextAnswerResponse> TextAnswers { get; set; } = [];
}