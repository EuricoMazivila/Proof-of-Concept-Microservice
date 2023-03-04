using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands.RequestModels;

public class LoginCommand : IRequest<LoginDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}