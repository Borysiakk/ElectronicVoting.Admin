using System.Security.Claims;
using ElectronicVoting.Admin.Infrastructure.JwtBearer;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using Microsoft.Extensions.Configuration;

namespace ElectronicVoting.Admin.Application.Services;

public interface IAuthenticationService
{
    JwtSettings GetJwtSettings();
    Task<(string token, string refreshToken)> GenerateToken(string email, CancellationToken cancellationToken);
}

public class AuthenticationService(
    ITokenService tokenService,
    IConfiguration configuration,
    IUserCredentialsRepository userCredentialsRepository)
    : IAuthenticationService
{
    private const string InvalidCredentialsMessage = "Invalid email or password";
    
    public JwtSettings GetJwtSettings()
    {
        return new JwtSettings
        {
            Key = GetConfigurationValue("Jwt:Key"),
            Issuer = GetConfigurationValue("Jwt:Issuer"),
            Audience = GetConfigurationValue("Jwt:Audience"),
            ExpirationInMinutes = GetJwtExpirationInMinutes(),
            RefreshTokenExpirationInDays = 30,
        };
    }

    public async Task<(string, string)> GenerateToken(string email, CancellationToken cancellationToken)
    {
        var user = await userCredentialsRepository.GetByEmailAsync(email, cancellationToken);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        return GenerateTokens(claims);
    }
    
    private (string token, string refreshToken) GenerateTokens(IList<Claim> voterClaims)
    {
        var jwtSettings = GetJwtSettings();
        var jwtToken = tokenService.GenerateJwtToken(jwtSettings, voterClaims);
        var refreshToken = tokenService.GenerateRefreshToken();
        return (jwtToken, refreshToken);
    }
    
    private int GetJwtExpirationInMinutes()
    {
        const string expirationKey = "Jwt:ExpirationMinutes";
        var expirationValue = GetConfigurationValue(expirationKey);
        return (int)TimeSpan.FromMinutes(double.Parse(expirationValue)).TotalMinutes;
    }
    private string GetConfigurationValue(string key)
    {
        return configuration[key] ?? throw new InvalidOperationException($"Configuration key '{key}' is missing.");
    }
}