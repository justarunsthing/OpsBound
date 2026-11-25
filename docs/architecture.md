# OpsBound architecture

Interview demo of an ERP-adjacent agent gateway. Business Central is mocked. Kafka is on the demo path: produce, project, exception, then a gated agent proposal.

Local broker is Redpanda in Docker Compose. Hosted demos use Azure Event Hubs (Kafka protocol) with the same topic names and the same Confluent.Kafka client. Azure AI Search is not part of this build.

## Components

| Component | Responsibility |
| --- | --- |
| OpsBound.Web | Blazor Interactive Server ops console |
| OpsBound.Gateway | Chat, tool dispatch, approvals, audit APIs |
| OpsBound.Agent | Prompt and tool loop. Mutating tools stop at approval |
| OpsBound.Rag | Chunk, embed, and retrieve policy docs with citations |
| OpsBound.MockBc | OData-shaped customers, invoices, items, stock, orders |
| OpsBound.Contracts | Event envelope, topic names, tool and approval DTOs |
| OpsBound.Publisher | Seed and burst POS and inventory events |
| OpsBound.Projector | Consume Kafka, update projected stock, emit exceptions |

## Topics

| Topic | Producers | Consumers | Key |
| --- | --- | --- | --- |
| `retail.pos.v1` | Publisher | Projector | `storeId:sku` or `orderId` |
| `retail.inventory.v1` | Publisher | Projector | `sku` |
| `ops.exceptions.v1` | Projector | Gateway, Blazor via API | `exceptionId` |
| `ops.agent.commands.v1` | Gateway after approval | Optional mock BC writer | `commandId` |
| `ops.dlq.v1` | Consumers on poison messages | Ops tooling | original key |

Records use a CloudEvents-style envelope: `id`, `type`, `source`, `time`, `data`. Samples and a JSON schema live in `contracts/events/`. Consumer group for inventory projection is `opsbound-inventory`, with idempotency on `event_id`. That consumer is a later phase.
