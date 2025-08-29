namespace CarAuctionApi.Models.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid CarAdId { get; set; }
        public CarAd CarAd { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}