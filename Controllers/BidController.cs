using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Services.Interfaces;

namespace CarAuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceBid([FromBody] BidCreateDto dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }
                var result = await _bidService.PlaceBidAsync(dto, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("carAd/{carAdId}")]
        public async Task<IActionResult> GetBidsByCarAdId(Guid carAdId)
        {
            try
            {
                var result = await _bidService.GetBidsByCarAdIdAsync(carAdId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("buy/{carAdId}")]
        [Authorize]
        public async Task<IActionResult> BuyFixedPrice(Guid carAdId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }
                var result = await _bidService.BuyFixedPriceAsync(carAdId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}