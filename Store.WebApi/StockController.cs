using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/controller")]
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
            return Ok(stockDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}