namespace OpsBound.Contracts.Events;

public sealed record PosSaleData
{
    public required string StoreId { get; init; }

    public required string Sku { get; init; }

    public required int Qty { get; init; }

    public required string OrderId { get; init; }

    /// <summary><c>sale</c> or <c>return</c>.</summary>
    public required string LineType { get; init; }
}

public sealed record InventoryAdjustmentData
{
    public required string StoreId { get; init; }

    public required string Sku { get; init; }

    public required int QtyDelta { get; init; }

    /// <summary>For example <c>adjust</c>, <c>receipt</c>, or <c>click-and-collect</c>.</summary>
    public required string Reason { get; init; }
}

public sealed record LowStockDetectedData
{
    public required string ExceptionId { get; init; }

    public required string StoreId { get; init; }

    public required string Sku { get; init; }

    public required int OnHandQty { get; init; }

    public required int Threshold { get; init; }
}

public sealed record AgentCommandData
{
    public required string CommandId { get; init; }

    public required string ApprovalId { get; init; }

    public required string CommandName { get; init; }

    public required string Sku { get; init; }

    public required int Qty { get; init; }
}

public sealed record DeadLetterData
{
    public required string OriginalTopic { get; init; }

    public required string OriginalKey { get; init; }

    public required string Reason { get; init; }

    public required int Attempts { get; init; }

    public required string Payload { get; init; }
}
