using Microsoft.EntityFrameworkCore;
using CarAuctionApi.Data;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;

namespace CarAuctionApi.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bid> CreateAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task<List<Bid>> GetByCarAdIdAsync(Guid carAdId)
        {
            return await _context.Bids
                .Include(b => b.User)
                .Where(b => b.CarAdId == carAdId)
                .ToListAsync();
        }

        public async Task<decimal?> GetHighestBidAmountAsync(Guid carAdId)
        {
            return await _context.Bids
                .Where(b => b.CarAdId == carAdId)
                .MaxAsync(b => (decimal?)b.Amount);
        }
    }
}