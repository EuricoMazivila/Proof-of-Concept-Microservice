using Application.Features.Users.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.Users.Commands.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
        RuleFor(x => x.FullName).NotEmpty().NotNull();
    }
}