using System.Text.Json.Serialization;


namespace OrderBookApi.Models;


public record OrderBookSide(decimal Price, decimal Amount);


public record OrderBookSnapshot
{
    public DateTimeOffset Timestamp { get; init; }
    public string Source { get; init; } = string.Empty;

    
    public List<OrderBookSide> Bids { get; init; } = new();
    public List<OrderBookSide> Asks { get; init; } = new();
}