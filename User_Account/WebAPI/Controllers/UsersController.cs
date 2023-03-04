using Application.DTOs;
using Application.Features.Users.Queries.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        return Ok(result);
    }
}