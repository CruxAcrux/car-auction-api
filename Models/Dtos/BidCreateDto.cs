namespace CarAuctionApi.Models.Dtos
{
    public class BidCreateDto
    {
        public Guid CarAdId { get; set; }
        public decimal Amount { get; set; }
    }
}