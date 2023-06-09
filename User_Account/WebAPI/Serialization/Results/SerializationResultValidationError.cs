﻿using System.Net;
using Application.Errors.CustomizeSpecificErrors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Serialization.Results;

public class SerializationResultValidationError : IResultSerializationStrategy
{
    public bool MustExecute(Result result)
    {
        return result.Errors.Any(e => e is ValidationError);
    }

    public bool MustExecute<TContent>(Result<TContent> result)
    {
        return MustExecute(result.ToResult());
    }

    public IActionResult Execute(Result result)
    {
        var error = (ValidationError)result.Errors.First(e => e is ValidationError);

        var validationProblemDetails = new ValidationProblemDetails(error.DictionaryFieldReasons)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "A validation error occurred."
        };
        return new ObjectResult(validationProblemDetails)
        {
            StatusCode = (int)HttpStatusCode.BadRequest
        };
    }

    public IActionResult Execute<TContent>(Result<TContent> result)
    {
        return Execute(result.ToResult());
    }
}