using Application.DTOs;
using Application.Features.Users.Commands.RequestModels;
using Application.Features.Users.Queries.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Serialization;

namespace WebAPI.Controllers;

public class UsersController : BaseController
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), 200)]
    public async Task<IActionResult> ListAllUsers()
    {
        var result = await _sender.Send(new ListAllUsersQuery());
        return this.SerializeResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await _sender.Send(command);
        return this.SerializeResult(result);
    }
}