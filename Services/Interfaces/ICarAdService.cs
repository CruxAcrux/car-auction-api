using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Models.Entities;

namespace CarAuctionApi.Services.Interfaces
{
    public interface ICarAdService
    {
        Task<CarAdDto> CreateAsync(CarAdCreateDto dto, string userId);
        Task<CarAdDto> GetByIdAsync(Guid id);
        Task<List<CarAdDto>> GetAllAsync(CarAdSearchDto searchDto);
        Task<CarAdDto> UpdateAsync(Guid id, CarAdCreateDto dto, string userId);
        Task DeleteAsync(Guid id, string userId);
        Task<List<CarBrandDto>> GetAllBrandsAsync();
        Task<List<CarModelDto>> GetModelsByBrandIdAsync(int brandId);
    }
}