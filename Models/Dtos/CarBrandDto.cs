namespace CarAuctionApi.Models.Dtos
{
    public class CarBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CarModelDto> Models { get; set; } = new List<CarModelDto>();
    }
}