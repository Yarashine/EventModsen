namespace EventModsen.Domain.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string entity) : base($"{entity} not found.") { }
}
