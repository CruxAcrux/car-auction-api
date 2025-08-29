namespace CarAuctionApi.Models.Entities
{
    public class Bid
    {
        public Guid Id { get; set; }
        public Guid CarAdId { get; set; }
        public CarAd CarAd { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}