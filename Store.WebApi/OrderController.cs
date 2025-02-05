using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/contoller")]
public class OrderController : ControllerBase 
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrderAsync(List<OrderProductDto> orderProductDto)
    {
        try
        {
            var order = await _orderService.PlaceOrderAsync(orderProductDto);
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