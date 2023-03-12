using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Features.Auth.Commands.RequestModels;

public class LoginCommand : IRequest<Result<LoginDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}