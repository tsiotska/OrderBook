using System.Net.Http.Json;
using OrderBookApi.Models;


namespace OrderBookApi.Services;

public class BitstampOrderBookProvider : IOrderBookProvider
{
    private readonly HttpClient _http;

    private const string
        Url = "https://www.bitstamp.net/api/v2/order_book/btcusd/";


    public BitstampOrderBookProvider(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }


    public async Task<OrderBookSnapshot> GetSnapshotAsync(CancellationToken ct = default)
    {

        var resp = await _http.GetFromJsonAsync<BitstampResponse>(Url, ct);
        if (resp == null) throw new InvalidOperationException("Failed to fetch order book");


        var snapshot = new OrderBookSnapshot
        {
            Timestamp = DateTimeOffset.UtcNow,
            Source = "Bitstamp",
            Bids = resp.Bids.Select(ParseSide).Select(s => new OrderBookSide(s.price, s.amount))
                .OrderByDescending(x => x.Price).ToList(),
            Asks = resp.Asks.Select(ParseSide).Select(s => new OrderBookSide(s.price, s.amount)).OrderBy(x => x.Price)
                .ToList()
        };


        return snapshot;


        static (decimal price, decimal amount) ParseSide(List<string> arr)
        {
            var price = decimal.Parse(arr[0], System.Globalization.CultureInfo.InvariantCulture);
            var amount = decimal.Parse(arr[1], System.Globalization.CultureInfo.InvariantCulture);
            return (price, amount);
        }
    }


    private class BitstampResponse
    {
        public List<List<string>> Bids { get; set; } = new();
        public List<List<string>> Asks { get; set; } = new();
    }
}