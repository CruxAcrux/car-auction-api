using CarAuctionApi.Models.Entities;

namespace CarAuctionApi.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task<Bid> CreateAsync(Bid bid);
        Task<List<Bid>> GetByCarAdIdAsync(Guid carAdId);
        Task<decimal?> GetHighestBidAmountAsync(Guid carAdId);
    }
}