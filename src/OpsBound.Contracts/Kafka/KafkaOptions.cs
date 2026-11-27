using Microsoft.Extensions.Configuration;

namespace OpsBound.Contracts.Kafka;

/// <summary>
/// Broker settings shared by Publisher and Projector.
/// Local Redpanda uses <c>localhost:19092</c> and <c>Plaintext</c>.
/// A hosted Event Hubs Kafka endpoint uses the same keys: <c>SaslSsl</c>,
/// mechanism <c>Plain</c> with username <c>$ConnectionString</c>, or mechanism <c>OAuthBearer</c>.
/// Put <c>SaslPassword</c> in user secrets or <c>Kafka__SaslPassword</c>, not in appsettings.
/// </summary>
public sealed class KafkaOptions
{
    public const string SectionName = "Kafka";

    public const string LocalBootstrapServers = "localhost:19092";

    public const string PlaintextProtocol = "Plaintext";

    public const string DefaultGroupId = "opsbound-inventory";

    public static readonly IReadOnlySet<string> SecurityProtocols = new HashSet<string>(StringComparer.Ordinal)
    {
        PlaintextProtocol,
        "Ssl",
        "SaslPlaintext",
        "SaslSsl",
    };

    public string BootstrapServers { get; set; } = "";

    public string SecurityProtocol { get; set; } = PlaintextProtocol;

    public string? SaslMechanism { get; set; }

    public string? SaslUsername { get; set; }

    public string? SaslPassword { get; set; }

    public string GroupId { get; set; } = DefaultGroupId;

    public static KafkaOptions Bind(IConfiguration configuration)
    {
        var options = new KafkaOptions();
        configuration.GetSection(SectionName).Bind(options);

        if (string.IsNullOrWhiteSpace(options.BootstrapServers))
        {
            throw new InvalidOperationException("Kafka:BootstrapServers is required.");
        }

        if (string.IsNullOrWhiteSpace(options.SecurityProtocol)
            || !SecurityProtocols.Contains(options.SecurityProtocol))
        {
            throw new InvalidOperationException(
                "Kafka:SecurityProtocol must be Plaintext, Ssl, SaslPlaintext, or SaslSsl.");
        }

        return options;
    }
}
