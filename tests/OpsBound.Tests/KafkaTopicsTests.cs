using OpsBound.Contracts.Events;

namespace OpsBound.Tests;

public class KafkaTopicsTests
{
    [Fact]
    public void Names_match_the_spec()
    {
        Assert.Equal(
            [
                "retail.pos.v1",
                "retail.inventory.v1",
                "ops.exceptions.v1",
                "ops.agent.commands.v1",
                "ops.dlq.v1",
            ],
            KafkaTopics.All);
    }
}
