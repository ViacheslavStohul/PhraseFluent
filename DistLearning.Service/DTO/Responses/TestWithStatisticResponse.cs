namespace DistLearning.Service.DTO.Responses;

public class TestWithStatisticResponse : TestResponse
{
    public List<CardWithStatisticsResponse> Cards { get; set; } = [];
}