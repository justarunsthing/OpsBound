using OpsBound.Contracts.Kafka;

namespace OpsBound.Projector;

public sealed class ProjectorWorker(ILogger<ProjectorWorker> logger, KafkaOptions kafka) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "OpsBound.Projector is idle. Broker {BootstrapServers}, protocol {SecurityProtocol}, group {GroupId}. Kafka consumption is added in a later phase.",
            kafka.BootstrapServers,
            kafka.SecurityProtocol,
            kafka.GroupId);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }
}
