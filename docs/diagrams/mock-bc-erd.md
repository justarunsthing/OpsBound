# Mock Business Central

In-memory seed for company `opsbound`. These are not Postgres tables.

```mermaid
erDiagram
    Customer ||--o{ SalesInvoice : billed
    Customer ||--o{ SalesOrder : places
    SalesOrder ||--|{ SalesOrderLine : contains
    Item ||--o{ SalesOrderLine : ordered
    Item ||--o{ ItemInventory : stocked

    Customer {
        string Id PK
        string DisplayName
        string Email
        bool CreditHold
    }
    SalesInvoice {
        string Id PK
        string CustomerId FK
        string Number
        decimal Amount
        date DueDate
        string Status
    }
    SalesOrder {
        string Number PK
        string CustomerId FK
        string Status
    }
    SalesOrderLine {
        string ItemNumber FK
        int Quantity
    }
    Item {
        string Number PK
        string DisplayName
        decimal UnitPrice
    }
    ItemInventory {
        string ItemNumber FK
        string StoreId
        int OnHand
    }
```

`SalesOrderLine` belongs to one order and names one item. `ItemInventory` is the on-hand quantity for an item at a store.
