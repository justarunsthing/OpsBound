using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace OpsBound.Persistence;

public sealed class Conversation
{
    public Guid Id { get; set; }

    public string Title { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}

public sealed class Message
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public Conversation Conversation { get; set; } = null!;

    public string Role { get; set; } = "";

    public string Content { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class ToolInvocation
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public string Name { get; set; } = "";

    public string ArgumentsJson { get; set; } = "";

    public string ResultHash { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class Approval
{
    public Guid Id { get; set; }

    public string Type { get; set; } = "";

    public string PayloadJson { get; set; } = "";

    public string Status { get; set; } = "Pending";

    public string? DecidedBy { get; set; }

    public DateTimeOffset? DecidedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class AuditEvent
{
    public Guid Id { get; set; }

    public Guid? ConversationId { get; set; }

    public Guid? ApprovalId { get; set; }

    public string Kind { get; set; } = "";

    public string DetailJson { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class ProcessedEvent
{
    public string EventId { get; set; } = "";

    public string Topic { get; set; } = "";

    public DateTimeOffset ProcessedAt { get; set; }
}

public sealed class ProjectedStock
{
    public string StoreId { get; set; } = "";

    public string Sku { get; set; } = "";

    public int Qty { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string LastEventId { get; set; } = "";
}

public sealed class OpsException
{
    public Guid Id { get; set; }

    public string Type { get; set; } = "";

    public string? Sku { get; set; }

    public string? CustomerId { get; set; }

    public string Status { get; set; } = "Open";

    public string? SourceEventId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class Document
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = "";

    public string Title { get; set; } = "";

    public DateTimeOffset IngestedAt { get; set; }

    public ICollection<Chunk> Chunks { get; set; } = new List<Chunk>();
}

public sealed class Chunk
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public Document Document { get; set; } = null!;

    public int Ordinal { get; set; }

    public string Content { get; set; } = "";

    /// <summary>1536 dimensions, matching text-embedding-3-small.</summary>
    [Column(TypeName = "vector(1536)")]
    public Vector? Embedding { get; set; }
}
