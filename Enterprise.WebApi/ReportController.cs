using AutoMapper;
using Enterprise.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;

namespace Enterprise.WebApi;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    
    private readonly IReportService _reportService;
    private readonly IMapper _mapper;
    
    public ReportController(IReportService reportService, IMapper mapper)
    {
        _reportService = reportService;
        _mapper = mapper;
    }
    
    [HttpGet("supplier-delivery-times")]
    public async Task<IActionResult> GetSupplierDeliveryTimes()
    {
        try
        {
            var deliveryTimes = await _reportService.GetSupplierDeliveryTimes();
            var deliveryTimesDto = _mapper.Map<List<SupplierDeliveryTimeDto>>(deliveryTimes);
            Console.WriteLine("Supplier delivery times delivered");
            return Ok(deliveryTimesDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine("error delivery reports");
            return BadRequest(ex.Message);
        }
    }
}