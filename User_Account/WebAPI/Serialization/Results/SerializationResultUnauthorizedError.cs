using System.Net;
using Application.Errors.CustomizeSpecificErrors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Serialization.Results;

public class SerializationResultUnauthorizedError : IResultSerializationStrategy
{
    public bool MustExecute(Result result)
    {
        return result.Errors.Any(e => e is UnauthorizedError);
    }

    public bool MustExecute<TContent>(Result<TContent> result)
    {
        return MustExecute(result.ToResult());
    }

    public IActionResult Execute(Result result)
    {
        var error = (UnauthorizedError)result.Errors.First(e => e is UnauthorizedError);

        var problemDetails = new ProblemDetails
        {
            Type = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1",
            Title = "Unauthorized",
            Detail = error.Message
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };
    }

    public IActionResult Execute<TContent>(Result<TContent> result)
    {
        return Execute(result.ToResult());
    }
}