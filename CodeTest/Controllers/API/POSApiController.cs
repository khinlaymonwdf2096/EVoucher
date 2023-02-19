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

        [Route("GetPurchases")]
        [HttpGet]
        public async Task<IEnumerable<PurchaseDTO>> Get()
        {

            List<Purchase> PurchaseList = await _posService.GetAllPurchases();
            List<PurchaseDTO> PurchaseDtoList = new List<PurchaseDTO>();
            PurchaseList.ForEach(x => PurchaseDtoList.Add(_mapper.Map<PurchaseDTO>(x)));

            return PurchaseDtoList;
        }



       

        [Route("ScanQRCode")]
        [HttpPost]
        public async Task<IActionResult> ScanQRCode([FromBody]PurchaseDTO purchaseDTO)
        {
            Purchase purchase = _mapper.Map<Purchase>(purchaseDTO);
            if (purchase.CouponId != 0)
            {
                var coupon = await _cmsService.GetEVoucherById(purchase.CouponId);

                DateTime currentDateTime = DateTime.Now;

                string key = "Coupon" + coupon.Id;

                if (currentDateTime >= coupon.StartDate && currentDateTime <= coupon.EndDate)
                {

                    string getqtyvalue = _distributedCache.GetString(key);
                    if (getqtyvalue != null)
                    {

                        int qty = int.Parse(getqtyvalue);
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
