namespace CarAuctionApi.Models.Dtos
{
    public class CarAdCreateDto
    {
        public int CarModelId { get; set; }
        public string RegistrationNumber { get; set; }
        public string TechnicalData { get; set; }
        public bool IsImported { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public decimal? FixedPrice { get; set; }
        public bool IsBiddable { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}