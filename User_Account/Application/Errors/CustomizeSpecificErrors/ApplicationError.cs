using FluentResults;

namespace Application.Errors.CustomizeSpecificErrors;

public class ApplicationError : Error
{
    public ApplicationError(string message) : base(message)
    {
    }
}