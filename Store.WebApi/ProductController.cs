using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/controller")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpPatch("change-price")]
    public async Task<IActionResult> ChangePriceAsync([FromQuery] Guid productId, [FromQuery] float newPrice)
    {
        try
        {
            var product = await _productService.ChangePrice(productId, newPrice);
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ShowAllProductsAsync()
    {
        try
        {
            var products = await _productService.ShowAllProductsAsync();
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}