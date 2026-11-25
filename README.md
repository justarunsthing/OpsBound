# OpsBound

A reusable .NET agent gateway and Blazor ops console that answers retail/ERP questions and proposes actions against mock Business Central data, driven by Kafka retail/ops events, with RAG citations, allow-listed tools, human approval, and full audit.

Business Central in this repo is a mock OData-shaped API. It is not Business Central Copilot and it does not use partner IP. Local Kafka is Redpanda in Docker, not a production cluster.

## Local infra

Requires the .NET 10 SDK (see `global.json`) and Docker Desktop.

```powershell
docker compose -f deploy/docker-compose.yml up -d
dotnet build
dotnet test
```

| Service | Address | Notes |
| --- | --- | --- |
| Kafka (Redpanda) | `localhost:19092` | Inside Compose, brokers use `redpanda:9092` |
| Redpanda Console | http://localhost:8080 | Topic browser |
| Postgres 16 + pgvector | `localhost:5432` | Database `opsbound`, user `opsbound`, password `opsbound` |

The Postgres password is for this local demo only. Do not reuse it anywhere else.

Compose creates the five topics `retail.pos.v1`, `retail.inventory.v1`, `ops.exceptions.v1`, `ops.agent.commands.v1`, and `ops.dlq.v1`. Envelope samples are in `contracts/events/`. See `docs/architecture.md` for producers, consumers, and partition keys.

`OpsBound.Publisher` and `OpsBound.Projector` start and idle. They do not produce or consume yet.

Stop the stack with `docker compose -f deploy/docker-compose.yml down`. Add `-v` to drop the local volumes.
