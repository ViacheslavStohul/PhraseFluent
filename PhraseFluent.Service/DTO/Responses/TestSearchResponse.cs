namespace PhraseFluent.Service.DTO.Responses;

public class TestSearchResponse
{
    public required IEnumerable<TestResponse> Items { get; set; }
    
    public int TotalItems { get; set; }
}