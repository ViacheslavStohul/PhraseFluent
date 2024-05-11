using PhraseFluent.DataAccess.Entities;

namespace PhraseFluent.DataAccess.Helpers;

public class TestSearcHelper
{
    public required IEnumerable<Test> Tests { get; set; }
    
    public int TotalItems { get; set; }
}