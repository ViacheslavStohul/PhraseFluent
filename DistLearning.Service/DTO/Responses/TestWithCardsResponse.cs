namespace DistLearning.Service.DTO.Responses;

public class TestWithCardsResponse : TestResponse
{
    public List<CardResponse> Cards { get; set; } = [];
}