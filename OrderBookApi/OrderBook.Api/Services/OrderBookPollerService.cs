using Microsoft.AspNetCore.SignalR;
using OrderBookApi.Hubs;
using OrderBookApi.Models;
using OrderBookApi.Repositories;


namespace OrderBookApi.Services;

public class OrderBookPollerService : BackgroundService
{
    private readonly IOrderBookProvider _provider;
    private readonly OrderBookManager _manager;
    private readonly IAuditRepository _audit;
    private readonly IHubContext<OrderBookHub> _hub;
    private readonly ILogger<OrderBookPollerService> _logger;


    public OrderBookPollerService(IOrderBookProvider provider, OrderBookManager manager, IAuditRepository audit,
        IHubContext<OrderBookHub> hub, ILogger<OrderBookPollerService> logger)
    {
        _provider = provider;
        _manager = manager;
        _audit = audit;
        _hub = hub;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderBookPoller started");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var snapshot = await _provider.GetSnapshotAsync(stoppingToken);
                _manager.SetSnapshot(snapshot);
                await _audit.AppendSnapshotAsync(snapshot, stoppingToken);
                await _hub.Clients.All.SendAsync("Snapshot", snapshot, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while polling order book");
            }
            
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}