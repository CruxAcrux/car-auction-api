namespace CarAuctionApi.Models.Entities
{
    public class CarAd
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }
        public string RegistrationNumber { get; set; }
        public string TechnicalData { get; set; }
        public bool IsImported { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public decimal? FixedPrice { get; set; }
        public bool IsBiddable { get; set; }
        public decimal? CurrentHighestBid { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AuctionEndDate { get; set; }
        public bool IsSold { get; set; }
        public string? BuyerId { get; set; }
        public ApplicationUser? Buyer { get; set; }
        public bool HasDownPayment { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Bid> Bids { get; set; } = new List<Bid>();
        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}