using Microsoft.Extensions.Configuration;

namespace ElectronicVoting.Admin.Infrastructure.JwtBearer;

public interface IJwtSettingsProvider
{
    JwtSettings GetJwtSettings();
}

public class JwtSettingsProvider(IConfiguration configuration) : IJwtSettingsProvider
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public JwtSettings GetJwtSettings()
    {
        return new JwtSettings
        {
            Key = GetConfigurationValue("Jwt:Key"),
            Issuer = GetConfigurationValue("Jwt:Issuer"),
            Audience = GetConfigurationValue("Jwt:Audience"),
            ExpirationInMinutes = GetJwtExpirationInMinutes()
        };
    }

    private string GetConfigurationValue(string key)
    {
        return _configuration[key] ?? throw new InvalidOperationException($"Configuration key '{key}' is missing.");
    }

    private int GetJwtExpirationInMinutes()
    {
        const string expirationKey = "Jwt:ExpirationMinutes";
        var expirationValue = GetConfigurationValue(expirationKey);
        return (int)TimeSpan.FromMinutes(double.Parse(expirationValue)).TotalMinutes;
    }
}
