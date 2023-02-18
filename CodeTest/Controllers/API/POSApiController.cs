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
    public class POSApiController : ControllerBase
    {
        private readonly ILogger<POSApiController> _logger;

        private readonly IMapper _mapper;

        private readonly IPOSService _posService;

        private readonly ICMSService _cmsService;

        private IDistributedCache _distributedCache;

        public POSApiController(ILogger<POSApiController> logger, IMapper mapper, IPOSService posService, ICMSService cmsService, IDistributedCache distributedCache)
        {
            _logger = logger;
            _mapper = mapper;
            _posService = posService;
            _cmsService = cmsService;
            _distributedCache = distributedCache;
        }



        //public POSApiController(ILogger<POSApiController> logger, IMapper mapper, IPOSService posService, ICMSService cmsService)
        //{
        //    _logger = logger;
        //    _mapper = mapper;
        //    _posService = posService;
        //    _cmsService = cmsService;
        //}



        //[Route("GetCoupons")]
        //[HttpPost]
        //public async Task<ActionResult> ScanQRCode([FromQuery] bool? showActive = null)
        //{
        //    // Coupon coupon = _mapper.Map<Coupon>(couponDTO);
        //   // return Ok(await _posService.GetAllEVouchers(showActive));
        //}

        //[Route("ScanQRCode")]
        //[HttpGet]
        //public async Task<IActionResult> ScanQRCode([FromQuery] int memberId, int couponId)
        //{

        //    var coupon = await _cmsService.GetEVoucherById(couponId);

        //    DateTime currentDateTime = DateTime.Now;

        //    string key = coupon.Id + "";

        //    if (currentDateTime >= coupon.StartDate && currentDateTime <= coupon.EndDate)
        //    {

        //        string aq = _distributedCache.GetString(key);
        //        if (aq != null)
        //        {

        //            int qty = int.Parse(aq);
        //            if (qty > 0)
        //            {
        //                _distributedCache.SetString(key, qty -1 + "");
        //            }
        //            else
        //            {
        //                return BadRequest("Reach Voucher Limit!");
        //            }

        //        }
        //        else
        //        {
        //            _distributedCache.SetString(key, coupon.AvailableQty - 1 + "");
        //        }
        //    }

        //    return Ok(_distributedCache.GetString(key));
        //}

        [Route("ScanQRCode")]
        [HttpPost]
        public async Task<IActionResult> ScanQRCode([FromBody]PurchaseDTO purchaseDTO)
        {
            Purchase purchase = _mapper.Map<Purchase>(purchaseDTO);

            var coupon = await _cmsService.GetEVoucherById(purchase.CouponId);

            DateTime currentDateTime = DateTime.Now;

            string key = coupon.Id + "";

            if (currentDateTime >= coupon.StartDate && currentDateTime <= coupon.EndDate)
            {

                string aq = _distributedCache.GetString(key);
                if (aq != null)
                {

                    int qty = int.Parse(aq);
                    if (qty > 0)
                    {
                        _distributedCache.SetString(key, qty - 1 + "");
                    }
                    else
                    {
                        return BadRequest("Reach Voucher Limit!");
                    }

                }
                else
                {
                    _distributedCache.SetString(key, coupon.AvailableQty - 1 + "");
                }
            }

            return Ok(await _posService.SavePurchase(purchase));
        }


        [Route("CreatePurchase")]
        [HttpPost]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseDTO purchaseDTO)
        {
            Purchase purchase = _mapper.Map<Purchase>(purchaseDTO);

            return Ok(await _posService.SavePurchase(purchase));
        }

        [Route("CreateMember")]
        [HttpPost]
        public async Task<ActionResult> CreateMember([FromBody] MemberDetailDTO memberDetailDTO)
        {
            MemberDetail memberDetail = _mapper.Map<MemberDetail>(memberDetailDTO);

            return Ok(await _posService.SaveMember(memberDetail));
        }

        //[Route("SaveAvailableQty")]
        //[HttpGet]
        //public async Task<ActionResult> SaveAvailableQty(int availableQty)
        //{
        //    //string aq = _distributedCache.GetString("AvailableQty");
        //   // await _distributedCache.SetStringAsync("AvailableQty", "9", default);
        //    return Ok("OK");
        //}

    }
}
