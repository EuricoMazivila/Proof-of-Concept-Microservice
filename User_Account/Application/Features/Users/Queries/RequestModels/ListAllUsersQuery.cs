using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Queries.RequestModels;

public class ListAllUsersQuery : IRequest<Result<IReadOnlyList<UserDto>>>
{
    
}