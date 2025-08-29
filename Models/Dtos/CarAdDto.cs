namespace CarAuctionApi.Models.Dtos
{
    public class CarAdDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CarModelId { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string RegistrationNumber { get; set; }
        public string TechnicalData { get; set; }
        public bool IsImported { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public decimal? FixedPrice { get; set; }
        public bool IsBiddable { get; set; }
        public decimal? CurrentHighestBid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public bool IsSold { get; set; }
        public bool HasDownPayment { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}