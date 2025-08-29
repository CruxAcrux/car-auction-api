using CarAuctionApi.Data;
using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAuctionApi.Repositories
{
    public class CarAdRepository : ICarAdRepository
    {
        private readonly ApplicationDbContext _context;

        public CarAdRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CarAd carAd)
        {
            await _context.CarAds.AddAsync(carAd);
            await _context.SaveChangesAsync();
        }

        public async Task<CarAd> GetByIdAsync(Guid id)
        {
            return await _context.CarAds
                .Include(c => c.CarModel)
                .ThenInclude(m => m.CarBrand)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CarAd carAd)
        {
            _context.CarAds.Update(carAd);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var carAd = await _context.CarAds.FindAsync(id);
            if (carAd != null)
            {
                _context.CarAds.Remove(carAd);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsValidModelIdAsync(int modelId)
        {
            return await _context.CarModels.AnyAsync(m => m.Id == modelId);
        }

        public async Task<bool> UserOwnsAdAsync(Guid id, string userId)
        {
            return await _context.CarAds.AnyAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<IQueryable<CarAd>> GetAllAsync(CarAdSearchDto searchDto)
        {
            var query = _context.CarAds
                .Include(c => c.CarModel)
                .ThenInclude(m => m.CarBrand)
                .Include(c => c.Images)
                .AsQueryable();

            // Apply UserId filter
            if (!string.IsNullOrEmpty(searchDto.UserId))
            {
                query = query.Where(ad => ad.UserId == searchDto.UserId);
            }

            // Apply other filters
            if (!string.IsNullOrEmpty(searchDto.Keyword))
            {
                query = query.Where(ad => ad.Description.Contains(searchDto.Keyword) || 
                                        ad.TechnicalData.Contains(searchDto.Keyword));
            }
            if (searchDto.CarBrandId.HasValue)
            {
                query = query.Where(ad => ad.CarModel.CarBrandId == searchDto.CarBrandId.Value);
            }
            if (searchDto.CarModelId.HasValue)
            {
                query = query.Where(ad => ad.CarModelId == searchDto.CarModelId.Value);
            }
            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(ad => ad.FixedPrice >= searchDto.MinPrice.Value);
            }
            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(ad => ad.FixedPrice <= searchDto.MaxPrice.Value);
            }
            if (searchDto.IsImported.HasValue)
            {
                query = query.Where(ad => ad.IsImported == searchDto.IsImported.Value);
            }
            if (searchDto.IsBiddable.HasValue)
            {
                query = query.Where(ad => ad.IsBiddable == searchDto.IsBiddable.Value);
            }

            return query;
        }

        public async Task<List<CarBrand>> GetAllBrandsAsync()
        {
            return await _context.CarBrands
                .Include(b => b.Models)
                .ToListAsync();
        }

        public async Task<List<CarModel>> GetModelsByBrandIdAsync(int brandId)
        {
            return await _context.CarModels
                .Where(m => m.CarBrandId == brandId)
                .ToListAsync();
        }
    }
}