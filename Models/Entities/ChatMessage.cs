namespace CarAuctionApi.Models.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid CarAdId { get; set; }
        public CarAd CarAd { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}