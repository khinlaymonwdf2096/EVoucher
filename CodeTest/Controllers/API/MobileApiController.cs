using AutoMapper;
using CodeTest.DTO;
using CodeTest.IServices;
using CodeTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CodeTest.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileApiController : ControllerBase
    {
        private readonly ILogger<POSApiController> _logger;

        private readonly IMapper _mapper;

        private readonly IPOSService _posService;

        private IDistributedCache _distributedCache;

        public MobileApiController(ILogger<POSApiController> logger, IMapper mapper, IPOSService posService, IDistributedCache distributedCache)
        {
            _logger = logger;
            _mapper = mapper;
            _posService = posService;
            _distributedCache = distributedCache;
        }

        [Route("Register")]
        [HttpGet]
        public async Task<ActionResult> RegisterWithOTP([FromQuery] AuthenticateOTPRequest authenticateOTPRequest)
        {
            if(authenticateOTPRequest.MobileNo=="09123456789" && authenticateOTPRequest.Otp == "1234")
            {
                return Ok("Registration Successful!");
            }
            else
            {
                return BadRequest("Registration Fail!");
            }
        }

        [Route("GetPurchasesbyMemberId")]
        [HttpGet]
        public async Task<IEnumerable<Purchase>> Get([FromQuery] int memberId)
        {
            List<Purchase> PurchaseList = await _posService.GetAllPurchases();
 

            return PurchaseList.Where(x=>x.MemberId==memberId).ToList();
        }

        [Route("GetTotalPointbyMemberId")]
        [HttpGet]
        public async Task<ActionResult> GetTotalPointbyMemberId([FromQuery] int memberId)
        {
          return Ok("Total Point: "+_distributedCache.GetString("Member"+memberId));
        }

        //[Route("GetAvailableCouponCount")]
        //[HttpGet]
        //public async Task<ActionResult> GetAvailableCouponCount([FromQuery] int memberId)
        //{
        //    List<string> listKeys = new List<string>();
        //    using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,allowAdmin=true"))
        //    {
        //        var keys = redis.GetServer("localhost", 6379).Keys();
        //        listKeys.AddRange(keys.Select(key => (string)key).ToList());

        //    }
        //    return Ok(listKeys);
        //}

    }
}
