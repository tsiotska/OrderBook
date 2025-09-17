using OrderBookApi.Models;

namespace OrderBookApi.Services;

public class OrderBookManager
{
    private OrderBookSnapshot? _latest;
    private readonly object _lock = new();

    public void SetSnapshot(OrderBookSnapshot snapshot)
    {
        lock (_lock)
        {
            _latest = snapshot;
        }
    }

    public OrderBookSnapshot? GetLatest()
    {
        lock (_lock)
        {
            return _latest;
        }
    }
    
    public QuoteResult CalculateQuote(OrderBookSnapshot snapshot, decimal btcAmount)
    {
        decimal remaining = btcAmount;
        decimal totalEur = 0m;
        
        foreach (var ask in snapshot.Asks.OrderBy(a => a.Price))
        {
            if (remaining <= 0) break;

            var take = Math.Min(remaining, ask.Amount);
            totalEur += take * ask.Price;
            remaining -= take;
        }

        if (remaining > 0)
        {
            throw new InvalidOperationException(
                $"Not enough liquidity in order book to fulfill {btcAmount} BTC (short by {remaining} BTC).");
        }

        decimal avgPrice = totalEur / btcAmount;
        return new QuoteResult(btcAmount, totalEur, avgPrice);
    }
}

public record QuoteResult(decimal BtcAmount, decimal TotalEur, decimal AveragePrice);