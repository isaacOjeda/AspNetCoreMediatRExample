using MediatR;
using MediatrExample.ApplicationCore.Common.Exceptions;
using MediatrExample.ApplicationCore.Common.Services;
using MediatrExample.ApplicationCore.Domain;
using MediatrExample.ApplicationCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediatrExample.ApplicationCore.Features.Auth;
public class RefreshTokenCommand : IRequest<RefreshTokenCommandResponse>
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
    private readonly MyAppDbContext _context;
    private readonly AuthService _authService;

    public RefreshTokenCommandHandler(MyAppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(q => q.RefreshTokenValue == request.RefreshToken);

        if (refreshToken is null ||
            refreshToken.Active == false ||
            refreshToken.Used ||
            refreshToken.Expiration <= DateTime.UtcNow)
        {
            throw new ForbiddenAccessException();
        }

        refreshToken.Used = true;

        var user = await _context.Users.FindAsync(refreshToken.UserId);

        if (user is null)
        {
            throw new ForbiddenAccessException();
        }

        var jwt = await _authService.GenerateAccessToken(user);

        var newRefreshToken = new RefreshToken
        {
            Active = true,
            Expiration = DateTime.UtcNow.AddDays(7),
            RefreshTokenValue = Guid.NewGuid().ToString("N"),
            Used = false,
            UserId = user.Id
        };

        _context.Add(newRefreshToken);

        await _context.SaveChangesAsync();

        return new RefreshTokenCommandResponse
        {
            AccessToken = jwt,
            RefreshToken = newRefreshToken.RefreshTokenValue
        };
    }
}

public class RefreshTokenCommandResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}