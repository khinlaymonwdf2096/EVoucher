using AutoMapper;
using CodeTest.DTO;
using CodeTest.IServices;
using CodeTest.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeTest.Controllers.API
{


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CMSApiController : ControllerBase
    {

        private readonly ILogger<CMSApiController> _logger;

        private readonly IMapper _mapper;

        private readonly ICMSService _cmsService;

        public CMSApiController(ILogger<CMSApiController> logger, IMapper mapper, ICMSService cmsService)
        {
            _logger = logger;
            _mapper = mapper;
            _cmsService = cmsService;
        }

        [Route("GetCoupons")]
        [HttpGet]
        public async Task<ActionResult> GetCoupons([FromQuery] bool? showActive = null)
        {
           // Coupon coupon = _mapper.Map<Coupon>(couponDTO);
            return Ok(await _cmsService.GetAllEVouchers(showActive));
        }

        [Route("CreateCoupon")]
        [HttpPost]
        public async Task<ActionResult> CreateEVoucher([FromBody] CouponDTO couponDTO)
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDTO);
            return Ok(await _cmsService.SaveCoupon(coupon));
        }

        [Route("UpdateCoupon")]
        [HttpPost]
        public async Task<ActionResult> UpdateCoupon([FromBody] CouponDTO couponDTO)
        {
           

            var result = await _cmsService.GetEVoucherById(couponDTO.Id);
            if (result == null)
            {
                return NotFound("EVoucher Not Found");
            }
            else
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDTO);
                return Ok(await _cmsService.UpdateCoupon(coupon.Id, coupon));
            }
        }

        [Route("GetMemberList")]
        [HttpGet]
        public async Task<ActionResult> GetMemberList()
        {
            // Coupon coupon = _mapper.Map<Coupon>(couponDTO);
            return Ok(await _cmsService.GetAllMemberList());
        }

    }
}
