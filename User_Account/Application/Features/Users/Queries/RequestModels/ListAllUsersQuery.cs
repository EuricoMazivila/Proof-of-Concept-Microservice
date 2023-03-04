using Application.DTOs;
using MediatR;

namespace Application.Features.Users.Queries.RequestModels;

public class ListAllUsersQuery : IRequest<IReadOnlyList<UserDto>>
{
    
}