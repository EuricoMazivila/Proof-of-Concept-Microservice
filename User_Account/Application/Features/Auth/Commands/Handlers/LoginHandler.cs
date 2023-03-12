using Application.DTOs;
using Application.Errors;
using Application.Features.Auth.Commands.RequestModels;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Commands.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginDto>>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IJwtGenerator jwtGenerator, IMapper mapper, ILogger<LoginHandler> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            _logger.LogError("Login Failed");
            return Results.UnauthorizedError("Login Failed");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Results.UnauthorizedError("Login Failed");

        var loginDto = _mapper.Map<AppUser, LoginDto>(user);
        loginDto.Token = await _jwtGenerator.CreateToken(user, cancellationToken);
        return Result.Ok(loginDto);
    }
}