using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarAuctionApi.Data;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;

namespace CarAuctionApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task CreateAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task AddRefreshTokenAsync(string userId, string refreshToken, DateTime expiryDate)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = expiryDate,
                IsRevoked = false
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetRefreshTokenAsync(string userId)
        {
            var refreshToken = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(rt => rt.CreatedAt)
                .Select(rt => rt.Token)
                .FirstOrDefaultAsync();
            return refreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();
            return storedToken != null;
        }

        public async Task<ApplicationUser> FindByIdAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> FindByRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .Where(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
                .Include(rt => rt.User)
                .FirstOrDefaultAsync();
            return refreshTokenEntity?.User;
        }
    }
}