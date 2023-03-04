using Application.DTOs;
using Application.Features.Auth.Commands.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AuthController : BaseController
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginDto), 200)]
    public async Task<IActionResult> Login(LoginCommand loginCommand, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(loginCommand, cancellationToken);
        return Ok(result);
    }
}