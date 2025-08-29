namespace CarAuctionApi.Models.Entities
{
    public class CarBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CarModel> Models { get; set; } = new List<CarModel>();
    }
}