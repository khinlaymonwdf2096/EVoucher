using AutoMapper;
using CodeTest.DTO;
using CodeTest.IServices;
using CodeTest.Models;
using CodeTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeTest.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointApiController : ControllerBase
    {
        private readonly ILogger<POSApiController> _logger;

        private readonly IMapper _mapper;

        private readonly IPointService _pointService;

        public PointApiController(ILogger<POSApiController> logger, IMapper mapper, IPointService pointService)
        {
            _logger = logger;
            _mapper = mapper;
            _pointService = pointService;
        }

        [Route("CalculatePoint")]
        [HttpPost]
        public async Task<ActionResult> CalculatePoint([FromBody] PurchaseDTO purchaseDTO)
        {
            Purchase purchase = _mapper.Map<Purchase>(purchaseDTO);
            return Ok(await _pointService.CalculatePoint(purchase));
        }



    }
}
