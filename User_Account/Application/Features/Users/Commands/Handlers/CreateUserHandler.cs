using Application.DTOs;
using Application.Errors;
using Application.Features.Users.Commands.RequestModels;
using Application.Interfaces;
using Domain;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Application.Features.Users.Commands.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEventBus _eventBus;

    public CreateUserHandler(UserManager<AppUser> userManager, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
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

        var userResult = new UserDto
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        
        var userCreated = new UserCreatedEvent
        {
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        
        await _eventBus.SendAsync(userCreated, "userCreatedQueue",cancellationToken);
        
        return Result.Ok(userResult);
    }
}