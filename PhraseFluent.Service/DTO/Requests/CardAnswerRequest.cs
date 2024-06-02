namespace PhraseFluent.Service.DTO.Requests;

public class CardAnswerRequest
{
    public required Guid CardUuid { get; set; }
    
    public ICollection<Guid>? PickedOptions { get; set; }
    
    public string? AnswerString { get; set; }
}