namespace OpsBound.Contracts.Events;

/// <summary>
/// Kafka topic names from the OpsBound spec. Local Redpanda and hosted Event Hubs use the same names.
/// </summary>
public static class KafkaTopics
{
    /// <summary>POS sale and return lines. Partition key: <c>storeId:sku</c> or <c>orderId</c>.</summary>
    public const string RetailPos = "retail.pos.v1";

    /// <summary>Stock adjust, receipt, and click-and-collect. Partition key: <c>sku</c>.</summary>
    public const string RetailInventory = "retail.inventory.v1";

    /// <summary>Low stock and other ops signals. Partition key: <c>exceptionId</c>.</summary>
    public const string OpsExceptions = "ops.exceptions.v1";

    /// <summary>Approved agent actions. Partition key: <c>commandId</c>.</summary>
    public const string OpsAgentCommands = "ops.agent.commands.v1";

    /// <summary>Poison messages. Partition key: the original record key.</summary>
    public const string OpsDlq = "ops.dlq.v1";

    public static readonly IReadOnlyList<string> All =
    [
        RetailPos,
        RetailInventory,
        OpsExceptions,
        OpsAgentCommands,
        OpsDlq,
    ];
}

/// <summary>CloudEvents <c>type</c> values carried on <see cref="EventEnvelope{TData}"/>.</summary>
public static class EventTypes
{
    public const string PosSale = "retail.pos.sale";
    public const string InventoryAdjusted = "retail.inventory.adjusted";
    public const string LowStockDetected = "ops.exception.low-stock-detected";
    public const string AgentCommand = "ops.agent.command";
    public const string DeadLetter = "ops.dlq.poison";
}
