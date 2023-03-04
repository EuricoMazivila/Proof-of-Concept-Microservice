using Application.DTOs;
using Application.Features.Users.Queries.RequestModels;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.Handlers;

public class ListAllUsersHandler : IRequestHandler<ListAllUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public ListAllUsersHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<UserDto>> Handle(ListAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .ToListAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<AppUser>, IReadOnlyList<UserDto>>(users);
    }
}