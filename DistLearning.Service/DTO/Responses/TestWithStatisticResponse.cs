namespace DistLearning.Service.DTO.Responses;

public class TestWithStatisticResponse : TestResponse
{
    public List<CardWithStatisticsResponse> Cards { get; set; } = [];
    
    public int CompletedAttempts { get; set; }
}