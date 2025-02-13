using AutoMapper;
using Domain.StoreSystem.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/orders")]
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
            var orderDto  = _mapper.Map<OrderDto>(order);
            Console.WriteLine("Order placed");
            return Ok(orderDto);
        }
        catch (Exception e)
        {
            Console.WriteLine("Order not placed");
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrderAsync([FromQuery] long[] orderIds)
    {
        try
        {
            var orders = await _orderService.ShowOrders(orderIds.ToList());
            var oderDto = _mapper.Map<List<OrderDto>>(orders);
            Console.WriteLine("Order requested");
            return Ok(oderDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPatch("roll-received-order")]
    public async Task<IActionResult> RollReceivedOrderAsync([FromQuery] long orderSupplierId)
    {
        try
        {
            await _orderService.RollReceivedOrderAsync(orderSupplierId);
            Console.WriteLine("Order received");
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine("Order not received");
            return BadRequest(e.Message);
        }
    }
}