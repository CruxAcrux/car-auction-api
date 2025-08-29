using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarAuctionApi.Repositories.Interfaces
{
    public interface ICarAdRepository
    {
        Task CreateAsync(CarAd carAd);
        Task<CarAd> GetByIdAsync(Guid id);
        Task UpdateAsync(CarAd carAd);
        Task DeleteAsync(Guid id);
        Task<bool> IsValidModelIdAsync(int modelId);
        Task<bool> UserOwnsAdAsync(Guid id, string userId);
        Task<IQueryable<CarAd>> GetAllAsync(CarAdSearchDto searchDto); // Updated to accept searchDto
        Task<List<CarBrand>> GetAllBrandsAsync();
        Task<List<CarModel>> GetModelsByBrandIdAsync(int brandId);
    }
}