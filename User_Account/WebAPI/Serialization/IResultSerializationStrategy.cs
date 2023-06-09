﻿using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Serialization;

public interface IResultSerializationStrategy
{
    bool MustExecute(Result result);
    bool MustExecute<TContent>(Result<TContent> result);
    IActionResult Execute(Result result);
    IActionResult Execute<TContent>(Result<TContent> result);
}