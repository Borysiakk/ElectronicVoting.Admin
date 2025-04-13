using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IRefreshTokenRepository: IRepository<RefreshToken>
{
    public Task<RefreshToken> FindByTokenAndEmailAsync(string refreshToken, string email);
}

public class RefreshTokenRepository(ElectionDbContext dbContext) : Repository<RefreshToken>(dbContext), IRefreshTokenRepository
{
    public async Task<RefreshToken> FindByTokenAndEmailAsync(string refreshToken, string email)
    {
        return await DbSet.FirstOrDefaultAsync(a=>a.Email == email && a.Token == refreshToken);
    }
}