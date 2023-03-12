using Application.Errors.CustomizeSpecificErrors;
using FluentResults;

namespace Application.Errors;

public static class Results
{
    public static Result ApplicationError(string message)
    {
        return Result.Fail(new ApplicationError(message));
    }

    public static Result InternalError(string messageError = "Unknown error", Exception? exception = default)
    {
        Error error = new InternalError(messageError);

        if (exception is not null)
            error = error.CausedBy(exception);
        
        return Result.Fail(error);
    }

    public static Result ConflictError(string message)
    {
        return Result.Fail(new ConflictError($"The resource: {message} exist in database!"));
    }
    
    public static Result UnauthorizedError(string message)
    {
        return Result.Fail(new UnauthorizedError(message));
    }

    public static Result ValidationError(Dictionary<string, string[]> dictionaryFieldReasons)
    {
        return Result.Fail(new ValidationError(dictionaryFieldReasons));
    }
}