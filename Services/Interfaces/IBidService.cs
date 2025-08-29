using CarAuctionApi.Models.Dtos;

namespace CarAuctionApi.Services.Interfaces
{
    public interface IBidService
    {
        Task<BidDto> PlaceBidAsync(BidCreateDto dto, string userId);
        Task<List<BidDto>> GetBidsByCarAdIdAsync(Guid carAdId);
        Task<DownPaymentDto> BuyFixedPriceAsync(Guid carAdId, string buyerId);
    }
}