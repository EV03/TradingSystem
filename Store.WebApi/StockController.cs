using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/stocks")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly IMapper _mapper;
    
    public StockController(IStockService stockService, IMapper mapper)
    {
        _stockService = stockService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> ShowStockAsync()
    {
        try
        {
            var stock = await _stockService.GetStockReportAsync();
            var stockDto = _mapper.Map<List<StockDto>>(stock);
            Console.WriteLine("Stock report generated");
            return Ok(stockDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(e.Message);
            
        }
    }
}