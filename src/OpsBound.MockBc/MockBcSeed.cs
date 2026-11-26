namespace OpsBound.MockBc;

public static class MockBcSeed
{
    public const string CompanyId = "opsbound";

    public static readonly IReadOnlyList<Customer> Customers =
    [
        new("CUST-1001", "Harbour Market", "ap@harbour.example", false),
        new("CUST-1002", "Bright Goods", "finance@bright.example", false),
        new("CUST-1003", "Ledger Street", "accounts@ledger.example", true),
    ];

    public static readonly IReadOnlyList<SalesInvoice> SalesInvoices =
    [
        new("INV-2001", "CUST-1001", "INV-2001", 1250.00m, new DateOnly(2025, 11, 1), "Open"),
        new("INV-2002", "CUST-1001", "INV-2002", 480.00m, new DateOnly(2026, 8, 1), "Open"),
        new("INV-2003", "CUST-1002", "INV-2003", 90.00m, new DateOnly(2027, 1, 15), "Open"),
        new("INV-2004", "CUST-1001", "INV-2004", 50.00m, new DateOnly(2026, 1, 1), "Paid"),
        new("INV-2005", "CUST-1003", "INV-2005", 300.00m, new DateOnly(2026, 9, 1), "Open"),
    ];

    public static readonly IReadOnlyList<Item> Items =
    [
        new("SKU-100", "Wool beanie", 18.00m),
        new("SKU-200", "Canvas tote", 12.00m),
    ];

    public static readonly IReadOnlyList<ItemInventory> Inventory =
    [
        new("SKU-100", "STORE-01", 24),
        new("SKU-200", "STORE-01", 80),
    ];

    public static readonly IReadOnlyList<SalesOrder> SalesOrders =
    [
        new(
            "SO-1001",
            "CUST-1001",
            "Open",
            [new SalesOrderLine("SKU-100", 2)]),
    ];
}

public sealed record Customer(string Id, string DisplayName, string Email, bool CreditHold);

public sealed record SalesInvoice(
    string Id,
    string CustomerId,
    string Number,
    decimal Amount,
    DateOnly DueDate,
    string Status);

public sealed record Item(string Number, string DisplayName, decimal UnitPrice);

public sealed record ItemInventory(string ItemNumber, string StoreId, int OnHand);

public sealed record SalesOrder(
    string Number,
    string CustomerId,
    string Status,
    IReadOnlyList<SalesOrderLine> Lines);

public sealed record SalesOrderLine(string ItemNumber, int Quantity);
