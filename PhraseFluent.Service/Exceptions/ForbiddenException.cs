namespace PhraseFluent.Service.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(){}

    public ForbiddenException(string message) : base(message){}
}