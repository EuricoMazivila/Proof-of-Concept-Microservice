using FluentResults;

namespace Application.Errors.CustomizeSpecificErrors;

public class ConflictError : Error
{
    public ConflictError(string message) : base(message)
    {
    }
}