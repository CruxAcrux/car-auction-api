using CarAuctionApi.Models.Entities;

namespace CarAuctionApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task CreateAsync(ApplicationUser user, string password);
        Task AddRefreshTokenAsync(string userId, string refreshToken, DateTime expiryDate);
        Task<string> GetRefreshTokenAsync(string userId);
        Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
        Task<ApplicationUser> FindByIdAsync(string? userId);
        Task<ApplicationUser> FindByRefreshTokenAsync(string refreshToken);
    }
}