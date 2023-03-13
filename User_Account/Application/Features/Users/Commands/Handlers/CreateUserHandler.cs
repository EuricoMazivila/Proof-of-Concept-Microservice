using Application.DTOs;
using Application.Errors;
using Application.Features.Users.Commands.RequestModels;
using Domain;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly UserManager<AppUser> _userManager;

    public CreateUserHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Email = request.Email,
            FullName = request.FullName,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return Results.InternalError("Fail to create account");

        return Result.Ok(new UserDto
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        });
    }
}