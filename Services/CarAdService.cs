using Microsoft.AspNetCore.Http;
using CarAuctionApi.Data;
using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;
using CarAuctionApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApi.Services
{
    public class CarAdService : ICarAdService
    {
        private readonly ICarAdRepository _carAdRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public CarAdService(ICarAdRepository carAdRepository, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _carAdRepository = carAdRepository;
            _environment = environment;
            _context = context;
        }

        public async Task<CarAdDto> CreateAsync(CarAdCreateDto dto, string userId)
        {
            if (!await _carAdRepository.IsValidModelIdAsync(dto.CarModelId))
            {
                throw new Exception("Invalid car model ID");
            }

            if (dto.Images == null || !dto.Images.Any() || dto.Images.Count > 15)
            {
                throw new Exception("Car ad must have between 1 and 15 images");
            }

            if (dto.IsBiddable && !dto.AuctionEndDate.HasValue)
            {
                throw new Exception("AuctionEndDate is required when IsBiddable is true");
            }

            if (dto.IsBiddable && dto.AuctionEndDate <= DateTime.UtcNow)
            {
                throw new Exception("AuctionEndDate must be in the future");
            }

            var imageUrls = new List<string>();
            var uploadsPath = Path.Combine(_environment.WebRootPath, "Uploads");
            Directory.CreateDirectory(uploadsPath);

            foreach (var file in dto.Images)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadsPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    imageUrls.Add($"/Uploads/{fileName}");
                }
            }

            var carAd = new CarAd
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CarModelId = dto.CarModelId,
                RegistrationNumber = dto.RegistrationNumber,
                TechnicalData = dto.TechnicalData,
                IsImported = dto.IsImported,
                Equipment = dto.Equipment,
                Description = dto.Description,
                FixedPrice = dto.FixedPrice,
                IsBiddable = dto.IsBiddable,
                AuctionEndDate = dto.IsBiddable ? dto.AuctionEndDate : null,
                Images = imageUrls.Select(url => new Image { FilePath = url }).ToList(),
                CreatedAt = DateTime.UtcNow
            };

            await _carAdRepository.CreateAsync(carAd);
            return await MapToDtoAsync(carAd);
        }

        public async Task<CarAdDto> UpdateAsync(Guid id, CarAdCreateDto dto, string userId)
        {
            var carAd = await _carAdRepository.GetByIdAsync(id);
            if (carAd == null)
            {
                throw new Exception("Car ad not found");
            }

            if (!await _carAdRepository.UserOwnsAdAsync(id, userId))
            {
                throw new Exception("User does not own this ad");
            }

            if (!await _carAdRepository.IsValidModelIdAsync(dto.CarModelId))
            {
                throw new Exception("Invalid car model ID");
            }

            if (dto.Images == null || !dto.Images.Any() || dto.Images.Count > 15)
            {
                throw new Exception("Car ad must have between 1 and 15 images");
            }

            if (dto.IsBiddable && !dto.AuctionEndDate.HasValue)
            {
                throw new Exception("AuctionEndDate is required when IsBiddable is true");
            }

            if (dto.IsBiddable && dto.AuctionEndDate <= DateTime.UtcNow)
            {
                throw new Exception("AuctionEndDate must be in the future");
            }

            carAd.CarModelId = dto.CarModelId;
            carAd.RegistrationNumber = dto.RegistrationNumber;
            carAd.TechnicalData = dto.TechnicalData;
            carAd.IsImported = dto.IsImported;
            carAd.Equipment = dto.Equipment;
            carAd.Description = dto.Description;
            carAd.FixedPrice = dto.FixedPrice;
            carAd.IsBiddable = dto.IsBiddable;
            carAd.AuctionEndDate = dto.IsBiddable ? dto.AuctionEndDate : null;

            if (dto.Images.Any())
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "Uploads");
                Directory.CreateDirectory(uploadsPath);

                foreach (var image in carAd.Images.ToList())
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, image.FilePath.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }
                carAd.Images.Clear();

                foreach (var file in dto.Images)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(uploadsPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        carAd.Images.Add(new Image { FilePath = $"/Uploads/{fileName}" });
                    }
                }
            }

            await _carAdRepository.UpdateAsync(carAd);
            return await MapToDtoAsync(carAd);
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            var carAd = await _carAdRepository.GetByIdAsync(id);
            if (carAd == null)
            {
                throw new Exception("Car ad not found");
            }

            if (!await _carAdRepository.UserOwnsAdAsync(id, userId))
            {
                throw new Exception("User does not own this ad");
            }

            foreach (var image in carAd.Images)
            {
                var filePath = Path.Combine(_environment.WebRootPath, image.FilePath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await _carAdRepository.DeleteAsync(id);
        }

        public async Task<CarAdDto> GetByIdAsync(Guid id)
        {
            var carAd = await _carAdRepository.GetByIdAsync(id);
            if (carAd == null)
            {
                throw new Exception("Car ad not found");
            }
            return await MapToDtoAsync(carAd);
        }

        public async Task<List<CarAdDto>> GetAllAsync(CarAdSearchDto searchDto)
        {
            var query = await _carAdRepository.GetAllAsync(searchDto);

            // Apply UserId filter as a fallback
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

            var carAds = await query.ToListAsync();
            var result = new List<CarAdDto>();
            foreach (var carAd in carAds)
            {
                result.Add(await MapToDtoAsync(carAd));
            }
            return result;
        }

        public async Task<List<CarBrandDto>> GetAllBrandsAsync()
        {
            var brands = await _carAdRepository.GetAllBrandsAsync();
            return brands.Select(b => new CarBrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Models = b.Models.Select(m => new CarModelDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    CarBrandId = m.CarBrandId
                }).ToList()
            }).ToList();
        }

        public async Task<List<CarModelDto>> GetModelsByBrandIdAsync(int brandId)
        {
            var models = await _carAdRepository.GetModelsByBrandIdAsync(brandId);
            return models.Select(m => new CarModelDto
            {
                Id = m.Id,
                Name = m.Name,
                CarBrandId = m.CarBrandId
            }).ToList();
        }

        private async Task<CarAdDto> MapToDtoAsync(CarAd carAd)
        {
            var user = await _context.Users.FindAsync(carAd.UserId);
            return new CarAdDto
            {
                Id = carAd.Id,
                UserId = carAd.UserId,
                UserName = $"{user?.FirstName} {user?.LastName}",
                CarModelId = carAd.CarModelId,
                CarBrand = carAd.CarModel?.CarBrand?.Name,
                CarModel = carAd.CarModel?.Name,
                RegistrationNumber = carAd.RegistrationNumber,
                TechnicalData = carAd.TechnicalData,
                IsImported = carAd.IsImported,
                Equipment = carAd.Equipment,
                Description = carAd.Description,
                FixedPrice = carAd.FixedPrice,
                IsBiddable = carAd.IsBiddable,
                CurrentHighestBid = carAd.CurrentHighestBid,
                CreatedAt = carAd.CreatedAt,
                AuctionEndDate = carAd.AuctionEndDate,
                IsSold = carAd.IsSold,
                HasDownPayment = carAd.HasDownPayment,
                ImageUrls = carAd.Images.Select(i => i.FilePath).ToList()
            };
        }
    }
}