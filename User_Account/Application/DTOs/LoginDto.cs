namespace Application.DTOs;

public class LoginDto : UserDto
{
    public string Token { get; set; } = string.Empty;
}