using MediatR;
using MediatrExample.ApplicationCore.Common.Exceptions;
using MediatrExample.ApplicationCore.Common.Services;
using MediatrExample.ApplicationCore.Domain;
using MediatrExample.ApplicationCore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace MediatrExample.ApplicationCore.Features.Auth;
public class TokenCommand : IRequest<TokenCommandResponse>
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;

}


public class TokenCommandHandler : IRequestHandler<TokenCommand, TokenCommandResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly AuthService _authService;
    private readonly MyAppDbContext _context;

    public TokenCommandHandler(UserManager<User> userManager, AuthService authService, MyAppDbContext context)
    {
        _userManager = userManager;
        _authService = authService;
        _context = context;
    }

    public async Task<TokenCommandResponse> Handle(TokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new ForbiddenAccessException();
        }

        var jwt = await _authService.GenerateAccessToken(user);
        var newAccessToken = new RefreshToken
        {
            Active = true,
            Expiration = DateTime.UtcNow.AddDays(7),
            RefreshTokenValue = Guid.NewGuid().ToString("N"),
            Used = false,
            UserId = user.Id
        };

        _context.Add(newAccessToken);

        await _context.SaveChangesAsync();

        return new TokenCommandResponse
        {
            AccessToken = jwt,
            RefreshToken = newAccessToken.RefreshTokenValue
        };
    }
}

public class TokenCommandResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}