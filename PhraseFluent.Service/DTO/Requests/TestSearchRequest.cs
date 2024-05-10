namespace PhraseFluent.Service.DTO.Requests;

public class TestSearchRequest
{
    private int _page;

    public int Page
    {
        get => _page < 1 ? 1 : _page;
        set => _page = value;
    }

    private int _size;
    
    public int Size
    {
        get => _size > 50 ? 50 : _size;
        set => _size = value;
    }

    private string? _language;
    
    public string? Language 
    { 
        get => _language?.ToUpper();
        set => _language = value;
    }

    private string? _username;
    
    public string? Username 
    { 
        get => _username?.ToUpper();
        set => _username = value;
    }

    private string? _title;
    
    public string? Title 
    { 
        get => _title?.ToUpper();
        set => _title = value;
    }
}