using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;
using CarAuctionApi.Services.Interfaces;

namespace CarAuctionApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            await _userRepository.CreateAsync(user, dto.Password);

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.FindByEmailAsync(dto.Email);
            if (user == null || !await _userRepository.CheckPasswordAsync(user, dto.Password))
            {
                throw new Exception("Invalid credentials");
            }

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.FindByRefreshTokenAsync(refreshToken);
            Console.WriteLine($"User found for refresh token: {user?.Id ?? "null"}");
            if (user == null || !await _userRepository.ValidateRefreshTokenAsync(user.Id, refreshToken))
            {
                throw new Exception("Invalid refresh token");
            }

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(ApplicationUser user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            var expiryDate = DateTime.UtcNow.AddDays(
                _configuration.GetSection("TokenSettings:RefreshTokenExpirationDays").Get<int>());

            await _userRepository.AddRefreshTokenAsync(user.Id, refreshToken, expiryDate);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id
            };
        }

        private string GenerateAccessToken(ApplicationUser user)
        {
            var tokenSettings = _configuration.GetSection("TokenSettings");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var token = new JwtSecurityToken(
                issuer: tokenSettings["Issuer"],
                audience: tokenSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    tokenSettings.GetValue<int>("AccessTokenExpirationMinutes")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        // Removed GetPrincipalFromExpiredToken as it's not needed
    }
}