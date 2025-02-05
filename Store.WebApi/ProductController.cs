using Microsoft.AspNetCore.Mvc;
using Store.Application.service;

namespace Store.WebApi;

[ApiController]
[Route("api/contoller")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPatch("change-price")]
    public async Task<IActionResult> ChangePriceAsync([FromQuery] Guid cachedProductId, [FromQuery] float newPrice)
    {
        try
        {
            var product = await _productService.ChangePrice(cachedProductId, newPrice);
            return Ok(product);
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
            return Ok(products);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}