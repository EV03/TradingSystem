using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/contoller")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    
    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }
    
    [HttpGet]
    public async Task<IActionResult> ShowStockAsync()
    {
        try
        {
            var stock = await _stockService.GetStockReportAsync();
            return Ok(stock);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}