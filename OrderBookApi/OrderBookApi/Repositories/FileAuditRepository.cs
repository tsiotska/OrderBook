using System.Text.Json;
using OrderBookApi.Models;


namespace OrderBookApi.Repositories;

public class FileAuditRepository : IAuditRepository
{
    private readonly string _path;
    private readonly SemaphoreSlim _mutex = new(1, 1);


    public FileAuditRepository(string path)
    {
        _path = path;
        var dir = Path.GetDirectoryName(path) ?? ".";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
    }


    public async Task AppendSnapshotAsync(OrderBookSnapshot snapshot, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(snapshot);
        await _mutex.WaitAsync(ct);
        try
        {
            await File.AppendAllTextAsync(_path, json + Environment.NewLine, ct);
        }
        finally
        {
            _mutex.Release();
        }
    }
}