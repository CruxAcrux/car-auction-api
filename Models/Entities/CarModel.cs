namespace CarAuctionApi.Models.Entities
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarBrandId { get; set; }
        public CarBrand CarBrand { get; set; }
    }
}