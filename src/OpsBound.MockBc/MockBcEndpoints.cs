namespace OpsBound.MockBc;

public static class MockBcEndpoints
{
    public static WebApplication MapMockBc(this WebApplication app)
    {
        app.MapGet("/", () => Results.Ok(new { service = "OpsBound.MockBc", companyId = MockBcSeed.CompanyId }))
            .ExcludeFromDescription();

        var company = app.MapGroup("/api/v2.0/companies/{companyId}")
            .AddEndpointFilter(async (invocation, next) =>
            {
                var companyId = invocation.HttpContext.Request.RouteValues["companyId"]?.ToString();

                if (!string.Equals(companyId, MockBcSeed.CompanyId, StringComparison.Ordinal))
                {
                    return Results.NotFound();
                }

                return await next(invocation);
            });

        company.MapGet("/customers", () => Results.Ok(MockBcSeed.Customers))
            .WithName("GetCustomers")
            .WithTags("Customers");

        company.MapGet("/customers/{customerId}", (string customerId) =>
            {
                var customer = MockBcSeed.Customers.FirstOrDefault(c => c.Id == customerId);
                return customer is null ? Results.NotFound() : Results.Ok(customer);
            })
            .WithName("GetCustomer")
            .WithTags("Customers");

        company.MapGet("/salesInvoices", (string? status, bool? overdue, TimeProvider time) =>
            {
                IEnumerable<SalesInvoice> invoices = MockBcSeed.SalesInvoices;
                if (!string.IsNullOrWhiteSpace(status))
                {
                    invoices = invoices.Where(invoice =>
                        invoice.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                }

                if (overdue == true)
                {
                    var today = DateOnly.FromDateTime(time.GetUtcNow().UtcDateTime);
                    invoices = invoices.Where(invoice =>
                        invoice.Status.Equals("Open", StringComparison.OrdinalIgnoreCase)
                        && invoice.DueDate < today);
                }

                return Results.Ok(invoices);
            })
            .WithName("GetSalesInvoices")
            .WithTags("SalesInvoices");

        company.MapGet("/items", () => Results.Ok(MockBcSeed.Items))
            .WithName("GetItems")
            .WithTags("Items");

        company.MapGet("/items/{itemNumber}", (string itemNumber) =>
            {
                var item = MockBcSeed.Items.FirstOrDefault(i => i.Number == itemNumber);
                return item is null ? Results.NotFound() : Results.Ok(item);
            })
            .WithName("GetItem")
            .WithTags("Items");

        company.MapGet("/items/{itemNumber}/inventory", (string itemNumber) =>
            {
                if (MockBcSeed.Items.All(i => i.Number != itemNumber))
                {
                    return Results.NotFound();
                }

                var rows = MockBcSeed.Inventory.Where(row => row.ItemNumber == itemNumber);
                return Results.Ok(rows);
            })
            .WithName("GetItemInventory")
            .WithTags("Items");

        company.MapGet("/salesOrders", () => Results.Ok(MockBcSeed.SalesOrders))
            .WithName("GetSalesOrders")
            .WithTags("SalesOrders");

        company.MapGet("/salesOrders/{orderNumber}", (string orderNumber) =>
            {
                var order = MockBcSeed.SalesOrders.FirstOrDefault(o => o.Number == orderNumber);
                return order is null ? Results.NotFound() : Results.Ok(order);
            })
            .WithName("GetSalesOrder")
            .WithTags("SalesOrders");

        return app;
    }
}
