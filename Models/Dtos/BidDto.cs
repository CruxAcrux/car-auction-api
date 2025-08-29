namespace CarAuctionApi.Models.Dtos
{
    public class BidDto
    {
        public Guid Id { get; set; }
        public Guid CarAdId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}