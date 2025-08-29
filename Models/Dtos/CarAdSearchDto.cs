namespace CarAuctionApi.Models.Dtos
{
    public class CarAdSearchDto
    {
        public string? UserId { get; set; }
        public string? Keyword { get; set; }
        public int? CarBrandId { get; set; }
        public int? CarModelId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsImported { get; set; }
        public bool? IsBiddable { get; set; }
    }
}