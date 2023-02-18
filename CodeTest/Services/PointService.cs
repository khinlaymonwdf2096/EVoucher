﻿using CodeTest.IServices;
using CodeTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace CodeTest.Services
{
    public class PointService : IPointService
    {
        private readonly CouponDbContext _context;

        

    

        public PointService(CouponDbContext context)
        {
            _context = context;
        }

        public async Task<MemberDetail> CalculatePoint(Purchase purchase)
        {
            
            Purchase result = await _context.Purchases.AsNoTracking().FirstOrDefaultAsync(x => x.MemberId == purchase.MemberId);
            

            var item = await _context.Items.AsNoTracking().ToArrayAsync();

            decimal totalAmount =(decimal) item.Where(x=>x.isNonAlcohol).Sum(x => x.Amount);
            int mempoint = 10;
            mempoint =(int) totalAmount / 100 * 10;

            MemberDetail memberDetail = await _context.MemberDetails.AsNoTracking().FirstOrDefaultAsync(x => x.Id == purchase.MemberId);

            memberDetail.TotalPoint += mempoint;
            memberDetail.TotalPurchaseAmount += totalAmount;

            
            _context.MemberDetails.Update(memberDetail);
            await _context.SaveChangesAsync();

            return memberDetail;

                
        }


       



    }
}
