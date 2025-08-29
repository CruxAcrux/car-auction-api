using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarAuctionApi.Models.Entities;

namespace CarAuctionApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarAd> CarAds { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<DownPayment> DownPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<CarAd>()
                .HasMany(c => c.Images)
                .WithOne(i => i.CarAd)
                .HasForeignKey(i => i.CarAdId);

            builder.Entity<CarAd>()
                .HasMany(c => c.Bids)
                .WithOne(b => b.CarAd)
                .HasForeignKey(b => b.CarAdId);

            builder.Entity<CarAd>()
                .HasMany(c => c.ChatMessages)
                .WithOne(m => m.CarAd)
                .HasForeignKey(m => m.CarAdId);

            builder.Entity<CarModel>()
                .HasOne(m => m.CarBrand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.CarBrandId);

            builder.Entity<RefreshToken>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            builder.Entity<DownPayment>()
                .HasOne(dp => dp.CarAd)
                .WithMany()
                .HasForeignKey(dp => dp.CarAdId);

            builder.Entity<DownPayment>()
                .HasOne(dp => dp.Buyer)
                .WithMany()
                .HasForeignKey(dp => dp.BuyerId);
        }
    }
}