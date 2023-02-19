using Microsoft.EntityFrameworkCore;

namespace CodeTest.Models
{
    public class CouponDbContext:DbContext
    {
        public CouponDbContext(DbContextOptions<CouponDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<MemberDetail> MemberDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>()
            .Property(a => a.Id).IsConcurrencyToken();
            modelBuilder.Entity<Coupon>().ToTable("Coupons");
            modelBuilder.Entity<Purchase>()
            .Property(a => a.Id).IsConcurrencyToken();
            modelBuilder.Entity<Purchase>().ToTable("Purchases");
            modelBuilder.Entity<MemberDetail>()
            .Property(a => a.Id).IsConcurrencyToken();
            modelBuilder.Entity<MemberDetail>().ToTable("Member");
            modelBuilder.Entity<Item>().ToTable("Items");

        }

        //public DbSet<EVoucherSystem.Models.PurchaseEVoucherViewModel>? PurchaseEVoucherViewModel { get; set; }

        //public DbSet<EVoucherSystem.Dtos.EVoucherDto>? EVoucherDto { get; set; }
    }
}
