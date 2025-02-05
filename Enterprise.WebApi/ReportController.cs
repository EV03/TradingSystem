using AutoMapper;
using Enterprise.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;

namespace Enterprise.WebApi;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> GetSupplierDeliveryTimes([FromQuery] Guid enterpriseId)
    {
            var deliveryTimes = await _reportService.GetSupplierDeliveryTimes(enterpriseId);
            var deliveryTimesDto = _mapper.Map<List<SupplierDeliveryTimeDto>>(deliveryTimes);
            return Ok(deliveryTimesDto);
    }
}