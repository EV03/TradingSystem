using AutoMapper;
using Domain.StoreSystem.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/controller")]
public class OrderController : ControllerBase 
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    
    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }
    
    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrderAsync( [FromBody] List<OrderProductDto> orderProductDto)
    {
        try
        {
            var order = await _orderService.PlaceOrderAsync(_mapper.Map<List<OrderProduct>>(orderProductDto));
            return Ok(order);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrderAsync([FromQuery] Guid[] orderIds)
    {
        try
        {
            var orders = await _orderService.ShowOrders(orderIds.ToList());
            return Ok(orders);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPatch("roll-received-order")]
    public async Task<IActionResult> RollReceivedOrderAsync([FromQuery] Guid orderSupplierId)
    {
        try
        {
            await _orderService.RollReceivedOrderAsync(orderSupplierId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}