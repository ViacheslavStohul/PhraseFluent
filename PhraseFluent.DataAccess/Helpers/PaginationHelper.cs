using PhraseFluent.DataAccess.Entities;

namespace PhraseFluent.DataAccess.Helpers;

public class PaginationHelper<TEntity> where TEntity : BaseId
{
    public required IEnumerable<TEntity> Items { get; set; }
    
    public int TotalItems { get; set; }
}