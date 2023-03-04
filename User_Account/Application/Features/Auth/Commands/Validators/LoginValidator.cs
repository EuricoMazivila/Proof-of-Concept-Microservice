﻿using Application.Features.Auth.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.Auth.Commands.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().NotNull();
    }
}