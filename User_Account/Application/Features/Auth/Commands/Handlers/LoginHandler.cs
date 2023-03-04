using Application.DTOs;
using Application.Features.Auth.Commands.RequestModels;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, LoginDto>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;

    public LoginHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IJwtGenerator jwtGenerator, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
    }
    
    public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception("Login Failed");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            throw new Exception("Login Failed");

        var loginDto = _mapper.Map<AppUser, LoginDto>(user);
        loginDto.Token = await _jwtGenerator.CreateToken(user, cancellationToken);
        return loginDto;
    }
}