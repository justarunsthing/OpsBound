using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpsBound.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    public const string ConnectionStringName = "OpsBound";

    public static IServiceCollection AddOpsBoundPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' is missing.");

        services.AddDbContext<OpsBoundDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql => npgsql.UseVector()));

        return services;
    }
}
