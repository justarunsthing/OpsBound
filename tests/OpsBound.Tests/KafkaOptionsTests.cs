using Microsoft.Extensions.Configuration;
using OpsBound.Contracts.Kafka;

namespace OpsBound.Tests;

public class KafkaOptionsTests
{
    [Fact]
    public void Local_section_binds_plaintext_broker()
    {
        var options = KafkaOptions.Bind(Config(
            ("Kafka:BootstrapServers", "localhost:19092"),
            ("Kafka:SecurityProtocol", "Plaintext")));

        Assert.Equal("localhost:19092", options.BootstrapServers);
        Assert.Equal("Plaintext", options.SecurityProtocol);
        Assert.Equal("opsbound-inventory", options.GroupId);
        Assert.Null(options.SaslMechanism);
        Assert.Null(options.SaslUsername);
        Assert.Null(options.SaslPassword);
    }

    [Fact]
    public void Event_hubs_shape_binds_sasl_fields()
    {
        var options = KafkaOptions.Bind(Config(
            ("Kafka:BootstrapServers", "namespace.servicebus.windows.net:9093"),
            ("Kafka:SecurityProtocol", "SaslSsl"),
            ("Kafka:SaslMechanism", "Plain"),
            ("Kafka:SaslUsername", "$ConnectionString"),
            ("Kafka:SaslPassword", "not-a-real-key"),
            ("Kafka:GroupId", "opsbound-inventory")));

        Assert.Equal("SaslSsl", options.SecurityProtocol);
        Assert.Equal("Plain", options.SaslMechanism);
        Assert.Equal("$ConnectionString", options.SaslUsername);
        Assert.Equal("not-a-real-key", options.SaslPassword);
    }

    [Fact]
    public void Missing_bootstrap_servers_throws()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            KafkaOptions.Bind(Config(("Kafka:SecurityProtocol", "Plaintext"))));

        Assert.Contains("BootstrapServers", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Unknown_security_protocol_throws()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            KafkaOptions.Bind(Config(
                ("Kafka:BootstrapServers", "localhost:19092"),
                ("Kafka:SecurityProtocol", "Tls"))));

        Assert.Contains("SecurityProtocol", exception.Message, StringComparison.Ordinal);
    }

    private static IConfiguration Config(params (string Key, string? Value)[] values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values.Select(value => new KeyValuePair<string, string?>(value.Key, value.Value)))
            .Build();
    }
}
