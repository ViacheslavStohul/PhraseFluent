namespace PhraseFluent.Service.DTO.Responses;

public class PaginationResponse<TEntity>
{
    public required IEnumerable<TEntity> Items { get; set; }
    
    public int TotalItems { get; set; }
}