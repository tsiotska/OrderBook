using OrderBookApi.Models;


namespace OrderBookApi.Repositories;

public interface IAuditRepository
{
    Task AppendSnapshotAsync(OrderBookSnapshot snapshot, CancellationToken ct = default);
}