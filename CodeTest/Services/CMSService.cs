using CodeTest.IServices;
using CodeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeTest.Services
{
    public class CMSService : ICMSService
    {
        private readonly CouponDbContext _context;

        public CMSService(CouponDbContext context)
        {
            _context = context;
        }

        public async Task<List<Coupon>> GetAllEVouchers(bool? showActive = null)
        {
            if(showActive != null && showActive.HasValue)
            {
                return await _context.Coupons.AsNoTracking().Where(x => x.IsActive == showActive).ToListAsync();
            }
            else
            {
                return await _context.Coupons.AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<MemberDetail>> GetAllMemberList()
        {
            
            
                return await _context.MemberDetails.AsNoTracking().ToListAsync();
            
        }

        public async Task<Coupon> GetEVoucherById(int id)
        {
            Coupon result = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Coupon> SaveCoupon(Coupon coupon)
        {
            coupon.CouponCode = Guid.NewGuid();
            var data = await _context.Coupons.FirstOrDefaultAsync();
            if(data == null)
            {
                coupon.Id = 1;
            }
            else
            {
                coupon.Id = _context.Coupons.Max(x => x.Id) + 1;
            }
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon> UpdateCoupon(int id, Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();



            return coupon;
        }
    }
}
