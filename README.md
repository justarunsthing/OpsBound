# OpsBound

A reusable .NET agent gateway and Blazor ops console that answers retail/ERP questions and proposes actions against mock Business Central data, driven by Kafka retail/ops events, with RAG citations, allow-listed tools, human approval, and full audit.

Business Central in this repo is a mock OData-shaped API. It is not Business Central, it does not use a Business Central license, and it is not Business Central Copilot. Local Kafka is Redpanda in Docker, not a production cluster.

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
| Postgres 16 + pgvector | `localhost:5433` | Database `opsbound`, user `opsbound`, password `opsbound`. Host port 5433 leaves a local Postgres on 5432 alone. |

The Postgres password is for local development only. Do not reuse it anywhere else.

Compose creates the five topics `retail.pos.v1`, `retail.inventory.v1`, `ops.exceptions.v1`, `ops.agent.commands.v1`, and `ops.dlq.v1`. Envelope samples are in `contracts/events/`. See `docs/architecture.md` for producers, consumers, partition keys, and which projects reference each other.

`OpsBound.Publisher` and `OpsBound.Projector` start and idle. They do not produce or consume yet.

Stop the stack with `docker compose -f deploy/docker-compose.yml down`. Add `-v` to drop the local volumes.

## Mock Business Central

`OpsBound.MockBc` serves fixed seed data for company `opsbound`. There is no Business Central license and no connection to a real tenant.

```powershell
dotnet run --project src/OpsBound.MockBc
```

| | |
| --- | --- |
| API | http://localhost:5242/api/v2.0/companies/opsbound/customers |
| Health | http://localhost:5242/health |
| Scalar | http://localhost:5242/scalar |

Resources: `customers`, `salesInvoices` (`status`, `overdue`), `items`, `items/{number}/inventory`, `salesOrders`. Seed identities include `CUST-1001`, `INV-2001`, `SKU-100`, `STORE-01`, and `SO-1001`.

## Database

Gateway applies EF Core migrations on startup. The initial migration enables the `pgvector` `vector` extension and creates conversations, messages, tool invocations, approvals, audit events, processed events, projected stock, exceptions, documents, and chunks.

The database password is not stored in the repo. Put the Compose connection string in User Secrets once. `secrets.json` stays outside the project.

```powershell
dotnet user-secrets set "ConnectionStrings:OpsBound" "Host=localhost;Port=5433;Database=opsbound;Username=opsbound;Password=opsbound" --project src/OpsBound.Gateway
dotnet tool restore
dotnet ef database update --project src/OpsBound.Persistence
dotnet run --project src/OpsBound.Gateway
```

Gateway and Projector share that User Secrets id, so one `secrets.json` covers both. Gateway health: http://localhost:5092/health. Diagrams are in `docs/diagrams/`.

To browse the tables, register a second server in pgAdmin. Leave any existing PostgreSQL server on port 5432. The OpsBound server is host `localhost`, port `5433`, database `opsbound`, username `opsbound`, password `opsbound`. The tables are under `opsbound` → Schemas → `public`.

To stop for the day, press Ctrl+C in the Gateway and Mock BC terminals, then run `docker compose -f deploy/docker-compose.yml down`. Omit `-v` so the database volume remains. Next session: start Docker, run `docker compose -f deploy/docker-compose.yml up -d`, then start Gateway and Mock BC again. The User Secrets entry persists, and Gateway reports the database is already up to date.
