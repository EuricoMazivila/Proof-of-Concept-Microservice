using FluentResults;

namespace Application.Errors.CustomizeSpecificErrors;

public class UnauthorizedError : Error
{
    public UnauthorizedError(string message)
        : base(message)
    {
    }
}