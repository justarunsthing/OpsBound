# Postgres store

Tables created by `InitialCreate` in database `opsbound`. Only `Message` → `Conversation` and `Chunk` → `Document` are foreign keys. Other id columns are stored without a database constraint.

```mermaid
erDiagram
    Conversation ||--o{ Message : contains
    Document ||--o{ Chunk : contains

    Conversation {
        uuid Id PK
        varchar Title
        timestamptz CreatedAt
    }
    Message {
        uuid Id PK
        uuid ConversationId FK
        varchar Role
        text Content
        timestamptz CreatedAt
    }
    ToolInvocation {
        uuid Id PK
        uuid ConversationId
        varchar Name
        text ArgumentsJson
        varchar ResultHash
        timestamptz CreatedAt
    }
    Approval {
        uuid Id PK
        varchar Type
        text PayloadJson
        varchar Status
        varchar DecidedBy
        timestamptz DecidedAt
        timestamptz CreatedAt
    }
    AuditEvent {
        uuid Id PK
        uuid ConversationId
        uuid ApprovalId
        varchar Kind
        text DetailJson
        timestamptz CreatedAt
    }
    ProcessedEvent {
        varchar EventId PK
        varchar Topic
        timestamptz ProcessedAt
    }
    ProjectedStock {
        varchar StoreId PK
        varchar Sku PK
        int Qty
        timestamptz UpdatedAt
        varchar LastEventId
    }
    OpsException {
        uuid Id PK
        varchar Type
        varchar Sku
        varchar CustomerId
        varchar Status
        varchar SourceEventId
        timestamptz CreatedAt
    }
    Document {
        uuid Id PK
        varchar FileName
        varchar Title
        timestamptz IngestedAt
    }
    Chunk {
        uuid Id PK
        uuid DocumentId FK
        int Ordinal
        text Content
        vector Embedding
    }
```

`Chunk.Embedding` is `vector(1536)`. `ProjectedStock` is keyed by store and SKU. `OpsException` is stored in the table `Exceptions`.
