using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OpsBound.Persistence;

public sealed class OpsBoundDbContextFactory : IDesignTimeDbContextFactory<OpsBoundDbContext>
{
    public const string UserSecretsId = "a7c3e4f1-2b58-4d6e-9a10-8f0c6b2d4e71";

    public OpsBoundDbContext CreateDbContext(string[] args)
    {
        var connectionString = ResolveConnectionString();
        var options = new DbContextOptionsBuilder<OpsBoundDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.UseVector())
            .Options;

        return new OpsBoundDbContext(options);
    }

    private static string ResolveConnectionString()
    {
        var fromEnvironment = Environment.GetEnvironmentVariable("OPSBOUND_CONNECTION");

        if (!string.IsNullOrWhiteSpace(fromEnvironment))
        {
            return fromEnvironment;
        }

        var fromSecrets = new ConfigurationBuilder()
            .AddUserSecrets(UserSecretsId)
            .Build()
            .GetConnectionString(PersistenceServiceCollectionExtensions.ConnectionStringName);

        if (!string.IsNullOrWhiteSpace(fromSecrets))
        {
            return fromSecrets;
        }

        throw new InvalidOperationException(
            "Connection string 'OpsBound' was not found. Set it with " +
            "'dotnet user-secrets set \"ConnectionStrings:OpsBound\" \"<connection string>\" " +
            "--project src/OpsBound.Gateway', or set OPSBOUND_CONNECTION.");
    }
}
