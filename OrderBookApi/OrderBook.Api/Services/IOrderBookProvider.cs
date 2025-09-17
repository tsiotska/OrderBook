using OrderBookApi.Models;


namespace OrderBookApi.Services;


public interface IOrderBookProvider
{
    Task<OrderBookSnapshot> GetSnapshotAsync(CancellationToken ct = default);
}