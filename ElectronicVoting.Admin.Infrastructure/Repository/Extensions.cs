using Microsoft.Extensions.DependencyInjection;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public static class Extensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVoterRepository, VoterRepository>();
        services.AddScoped<IApproverRepository, ApproverRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IPaillierKeysRepository, PaillierKeysRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IVoterPublicKeyRepository, VoterPublicKeyRepository>();
        
        services.AddScoped<IElectionRepository, ElectionRepository>();
        services.AddScoped<IElectionVotersRepository, ElectionVotersRepository>();
        services.AddScoped<IElectionCandidatesRepository, ElectionCandidatesRepository>();
        services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
    }
}