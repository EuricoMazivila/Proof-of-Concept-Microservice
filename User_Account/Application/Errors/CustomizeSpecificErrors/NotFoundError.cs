using FluentResults;

namespace Application.Errors.CustomizeSpecificErrors;

public class NotFoundError : Error
{
    public NotFoundError(string name) : base(name)
    {
    }
}