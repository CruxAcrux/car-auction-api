namespace CarAuctionApi.Models.Entities
{
    public class DownPayment
    {
        public Guid Id { get; set; }
        public Guid CarAdId { get; set; }
        public CarAd CarAd { get; set; }
        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }
        public decimal Amount { get; set; } // 3000 SEK
        public bool IsForfeited { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}