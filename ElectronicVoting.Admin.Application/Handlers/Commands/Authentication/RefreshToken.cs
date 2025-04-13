using System.Security.Claims;
using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Services;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.JwtBearer;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Authentication;

public class RefreshToken : IRequest<Result<RefreshTokenDto>>
{
    public string Token { get; set; }
    public string TokenRefresh { get; set; }
}

public class RefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService,
    IAuthenticationService authenticationService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RefreshToken, Result<RefreshTokenDto>>
{
    private const string InvalidCredentialsMessage = "Invalid attempt!";

    public async Task<Result<RefreshTokenDto>> Handle(RefreshToken request, CancellationToken cancellationToken)
    {
        var jwtSettings = authenticationService.GetJwtSettings();

        var claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(request.Token, jwtSettings);
        var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(emailClaim))
            return Result.Fail<RefreshTokenDto>(InvalidCredentialsMessage);

        var refreshToken = await refreshTokenRepository.FindByTokenAndEmailAsync(request.TokenRefresh, emailClaim);

        if (refreshToken.Token != request.TokenRefresh)
            return Result.Fail<RefreshTokenDto>(InvalidCredentialsMessage);

        var tokens = await authenticationService.GenerateToken(emailClaim, cancellationToken);
        await UpdateRefreshToken(refreshToken, tokens.refreshToken, cancellationToken);
        
        return MapVoterToDto(refreshToken, tokens.token);
    }

    private async Task UpdateRefreshToken(Domain.Entities.RefreshToken refreshToken, string newRefreshToken, CancellationToken cancellationToken)
    {
        refreshToken.Token = newRefreshToken;
        refreshToken.ModifiedDate = DateTime.Now;
        refreshToken.Expires = DateTime.Now + TimeSpan.FromDays(30);
        await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
    
    private RefreshTokenDto MapVoterToDto(Domain.Entities.RefreshToken refreshToken, string token) => new()
    {
        Token = token,
        Expires = refreshToken.Expires,
        RefreshToken = refreshToken.Token,
    };
}