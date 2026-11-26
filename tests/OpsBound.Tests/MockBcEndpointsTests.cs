using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OpsBound.MockBc;

namespace OpsBound.Tests;

public class MockBcEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MockBcEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_returns_success()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Unknown_company_is_not_found()
    {
        var response = await _client.GetAsync("/api/v2.0/companies/other/customers");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Customers_include_the_seed_accounts()
    {
        var customers = await _client.GetFromJsonAsync<List<Customer>>(
            "/api/v2.0/companies/opsbound/customers");

        Assert.NotNull(customers);
        Assert.Contains(customers, customer => customer.Id == "CUST-1001" && !customer.CreditHold);
        Assert.Contains(customers, customer => customer.Id == "CUST-1003" && customer.CreditHold);
    }

    [Fact]
    public async Task Open_overdue_invoices_keep_paid_invoices_out()
    {
        var invoices = await _client.GetFromJsonAsync<List<SalesInvoice>>(
            "/api/v2.0/companies/opsbound/salesInvoices?status=Open&overdue=true");

        Assert.NotNull(invoices);
        Assert.Contains(invoices, invoice => invoice.Id == "INV-2001" && invoice.CustomerId == "CUST-1001");
        Assert.DoesNotContain(invoices, invoice => invoice.Id == "INV-2004");
        Assert.DoesNotContain(invoices, invoice => invoice.Id == "INV-2003");
    }

    [Fact]
    public async Task Sku_100_stock_and_sales_order_match_the_seed()
    {
        var inventory = await _client.GetFromJsonAsync<List<ItemInventory>>(
            "/api/v2.0/companies/opsbound/items/SKU-100/inventory");
        var order = await _client.GetFromJsonAsync<SalesOrder>(
            "/api/v2.0/companies/opsbound/salesOrders/SO-1001");

        Assert.NotNull(inventory);
        Assert.Contains(inventory, row => row.StoreId == "STORE-01" && row.OnHand == 24);
        Assert.NotNull(order);
        Assert.Equal("CUST-1001", order.CustomerId);
        Assert.Contains(order.Lines, line => line.ItemNumber == "SKU-100" && line.Quantity == 2);
    }
}
