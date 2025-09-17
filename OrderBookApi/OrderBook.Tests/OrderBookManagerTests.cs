using OrderBookApi.Models;
using OrderBookApi.Services;

namespace OrderBook.Tests.Services
{
    public class OrderBookManagerTests
    {
        [Fact]
        public void GetLatest_ReturnsNull_WhenNoSnapshotSet()
        {
            var manager = new OrderBookManager();

            var result = manager.GetLatest();

            Assert.Null(result);
        }

        [Fact]
        public void SetSnapshot_ThenGetLatest_ReturnsSameSnapshot()
        {
            var manager = new OrderBookManager();
            var snapshot = new OrderBookSnapshot
            {
                Timestamp = DateTimeOffset.UtcNow,
                Asks = new List<OrderBookSide> { new OrderBookSide(100m, 1m) },
                Bids = new List<OrderBookSide> { new OrderBookSide(99m, 1m) },
                Source = "Test"
            };

            manager.SetSnapshot(snapshot);
            var result = manager.GetLatest();

            Assert.NotNull(result);
            Assert.Equal(snapshot, result);
        }

        [Fact]
        public void CalculateQuote_ReturnsCorrectQuote_WhenLiquidityIsEnough()
        {
            var manager = new OrderBookManager();
            var snapshot = new OrderBookSnapshot
            {
                Asks = new List<OrderBookSide>
                {
                    new OrderBookSide(100m, 0.5m),
                    new OrderBookSide(105m, 1m)
                }
            };

            var result = manager.CalculateQuote(snapshot, 0.5m);

            Assert.Equal(0.5m, result.BtcAmount);
            Assert.Equal(50m, result.TotalEur); // 0.5 * 100
            Assert.Equal(100m, result.AveragePrice);
        }

        [Fact]
        public void CalculateQuote_SplitsAcrossAsks_WhenNeeded()
        {
            var manager = new OrderBookManager();
            var snapshot = new OrderBookSnapshot
            {
                Asks = new List<OrderBookSide>
                {
                    new OrderBookSide(100m, 0.5m),
                    new OrderBookSide(105m, 1m)
                }
            };

            var result = manager.CalculateQuote(snapshot, 1m);

            // First 0.5 BTC at 100, then 0.5 BTC at 105
            Assert.Equal(1m, result.BtcAmount);
            Assert.Equal(0.5m * 100m + 0.5m * 105m, result.TotalEur);
            Assert.Equal((0.5m * 100m + 0.5m * 105m) / 1m, result.AveragePrice);
        }

        [Fact]
        public void CalculateQuote_Throws_WhenLiquidityNotEnough()
        {
            var manager = new OrderBookManager();
            var snapshot = new OrderBookSnapshot
            {
                Asks = new List<OrderBookSide>
                {
                    new OrderBookSide(100m, 0.5m)
                }
            };

            Assert.Throws<InvalidOperationException>(() =>
                manager.CalculateQuote(snapshot, 1m)
            );
        }
    }
}
