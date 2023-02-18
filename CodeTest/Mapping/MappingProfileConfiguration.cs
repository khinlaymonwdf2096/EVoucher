using AutoMapper;
using CodeTest.DTO;
using CodeTest.Models;

namespace CodeTest.Mapping
{
    public class MappingProfileConfiguration:Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<Coupon, CouponDTO>().ReverseMap();

            CreateMap<Purchase, PurchaseDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<MemberDetail, MemberDetailDTO>().ReverseMap();

        }
        
    }
}
