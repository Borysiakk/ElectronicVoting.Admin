using System.Security.Claims;
using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Services;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.JwtBearer;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Authentication;

public class Login: IRequest<Result<LoginDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginHandler(IRefreshTokenRepository refreshTokenRepository, IAuthenticationService authenticationService, IUnitOfWork unitOfWork): IRequestHandler<Login, Result<LoginDto>>
{
    public async Task<Result<LoginDto>> Handle(Login request, CancellationToken cancellationToken)
    {
        var tokens = await authenticationService.GenerateToken(request.Email, cancellationToken);
        await SaveRefreshToken(tokens.refreshToken, request.Email, cancellationToken);
        
        return Result.Ok<LoginDto>(new LoginDto()
        {
            Token = tokens.token,
            TokenRefresh = tokens.refreshToken
        });
    }
    
    private async Task SaveRefreshToken(string token, string email, CancellationToken cancellationToken)
    {
        var refreshToken = new Domain.Entities.RefreshToken()
        {
            Token = token,
            Email = email,
            CreatedDate = DateTime.Now,
            Expires = DateTime.Now.AddDays(30),
        };
        
        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}

