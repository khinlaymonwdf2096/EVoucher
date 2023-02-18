using CodeTest.Models;

namespace CodeTest.IServices
{
    public interface ICMSService
    {
        public Task<List<Coupon>> GetAllEVouchers(bool? showActive = null);
        public Task<Coupon> GetEVoucherById(int id);
        public Task<Coupon> SaveCoupon(Coupon coupon);
        public Task<Coupon> UpdateCoupon(int id, Coupon coupon);

        public Task<List<MemberDetail>> GetAllMemberList();
    }
}