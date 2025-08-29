using Microsoft.EntityFrameworkCore;
using CarAuctionApi.Data;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;

namespace CarAuctionApi.Repositories
{
    public class DownPaymentRepository : IDownPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public DownPaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DownPayment> CreateAsync(DownPayment downPayment)
        {
            _context.DownPayments.Add(downPayment);
            await _context.SaveChangesAsync();
            return downPayment;
        }

        public async Task<DownPayment> GetByCarAdIdAndBuyerIdAsync(Guid carAdId, string buyerId)
        {
            return await _context.DownPayments
                .Include(dp => dp.Buyer)
                .FirstOrDefaultAsync(dp => dp.CarAdId == carAdId && dp.BuyerId == buyerId);
        }
    }
}