namespace CarAuctionApi.Models.Dtos
{
    public class DownPaymentDto
    {
        public Guid Id { get; set; }
        public Guid CarAdId { get; set; }
        public string BuyerId { get; set; }
        public string BuyerName { get; set; }
        public decimal Amount { get; set; }
        public bool IsForfeited { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}