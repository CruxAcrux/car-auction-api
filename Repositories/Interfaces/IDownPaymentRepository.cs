using CarAuctionApi.Models.Entities;

namespace CarAuctionApi.Repositories.Interfaces
{
    public interface IDownPaymentRepository
    {
        Task<DownPayment> CreateAsync(DownPayment downPayment);
        Task<DownPayment> GetByCarAdIdAndBuyerIdAsync(Guid carAdId, string buyerId);
    }
}