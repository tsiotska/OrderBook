using Microsoft.AspNetCore.Mvc;
using OrderBookApi.Models;
using OrderBookApi.Services;

namespace OrderBookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderBookController : ControllerBase
{
    private readonly OrderBookManager _manager;

    public OrderBookController(OrderBookManager manager)
    {
        _manager = manager;
    }
    
    [HttpGet("snapshot")]
    public ActionResult<OrderBookSnapshot> GetSnapshot()
    {
        var snapshot = _manager.GetLatest();
        if (snapshot == null)
            return NotFound("No snapshot available yet.");

        return Ok(snapshot);
    }
    
    [HttpPost("quote")]
    public ActionResult<QuoteResult> GetQuote([FromBody] QuoteRequest request)
    {
        if (request.BtcAmount <= 0)
            return BadRequest("BTC amount must be greater than zero.");

        var snapshot = _manager.GetLatest();
        if (snapshot == null)
            return NotFound("No snapshot available yet.");

        var result = _manager.CalculateQuote(snapshot, request.BtcAmount);
        return Ok(result);
    }

    public record QuoteRequest(decimal BtcAmount);

    public record QuoteResult(decimal BtcAmount, decimal TotalEur, decimal AveragePrice);
}