using AutoMapper;
using CodeTest.DTO;
using CodeTest.IServices;
using CodeTest.Models;
using CodeTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CodeTest.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointApiController : ControllerBase
    {
        private readonly ILogger<POSApiController> _logger;

        private readonly IMapper _mapper;

        private readonly IPointService _pointService;

        private IDistributedCache _distributedCache;

        public PointApiController(ILogger<POSApiController> logger, IMapper mapper, IPointService pointService, IDistributedCache distributedCache)
        {
            _logger = logger;
            _mapper = mapper;
            _pointService = pointService;
            _distributedCache = distributedCache;
        }

        [Route("CalculatePoint")]
        [HttpPost]
        public async Task<ActionResult> CalculatePoint([FromBody] PurchaseDTO purchaseDTO)
        {
            Purchase purchase = _mapper.Map<Purchase>(purchaseDTO);
            var memberDetail = await _pointService.CalculatePoint(purchase);
            _distributedCache.SetString("Member"+memberDetail.Id, memberDetail.TotalPoint+"");
            return Ok(memberDetail);
        }



    }
}
