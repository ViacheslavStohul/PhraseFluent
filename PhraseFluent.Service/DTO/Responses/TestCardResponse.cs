namespace PhraseFluent.Service.DTO.Responses;

public class TestCardResponse
{
    public required BaseCardResponse Card { get; set; }
    
    public Guid TestAttemptUuid { get; set; }
    
    public int Questions { get; set; }
    
    public int CurrentQuestion { get; set; }
}