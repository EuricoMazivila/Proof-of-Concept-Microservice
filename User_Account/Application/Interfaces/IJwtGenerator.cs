using Domain;

namespace Application.Interfaces;

public interface IJwtGenerator
{
    Task<string> CreateToken(AppUser user, CancellationToken cancellationToken = default);
}