using System.Text.Json;
using OpsBound.Contracts.Events;

namespace OpsBound.Tests;

public class EventEnvelopeTests
{
    [Fact]
    public void Serializes_cloud_events_fields_in_camel_case()
    {
        var envelope = new EventEnvelope<PosSaleData>
        {
            Id = "evt-pos-1001",
            Type = EventTypes.PosSale,
            Source = "opsbound://publisher",
            Time = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero),
            Data = new PosSaleData
            {
                StoreId = "STORE-01",
                Sku = "SKU-100",
                Qty = 2,
                OrderId = "SO-1001",
                LineType = "sale",
            },
        };

        var json = JsonSerializer.Serialize(envelope, JsonSerializerOptions.Web);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("evt-pos-1001", root.GetProperty("id").GetString());
        Assert.Equal(EventTypes.PosSale, root.GetProperty("type").GetString());
        Assert.Equal("opsbound://publisher", root.GetProperty("source").GetString());
        Assert.Equal("SKU-100", root.GetProperty("data").GetProperty("sku").GetString());
        Assert.Equal(2, root.GetProperty("data").GetProperty("qty").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(root.GetProperty("time").GetString()));
    }
}
