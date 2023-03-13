using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.RequestModels;

public class CreateUserCommand : IRequest<Result<UserDto>>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}