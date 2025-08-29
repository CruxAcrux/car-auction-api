using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Services.Interfaces;

namespace CarAuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarAdController : ControllerBase
    {
        private readonly ICarAdService _carAdService;

        public CarAdController(ICarAdService carAdService)
        {
            _carAdService = carAdService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CarAdCreateDto dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }
                var result = await _carAdService.CreateAsync(dto, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _carAdService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] CarAdSearchDto searchDto)
        {
            try
            {
                var result = await _carAdService.GetAllAsync(searchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] CarAdCreateDto dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }
                var result = await _carAdService.UpdateAsync(id, dto, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }
                await _carAdService.DeleteAsync(id, userId);
                return Ok(new { Message = "Car ad deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var result = await _carAdService.GetAllBrandsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("brands/{brandId}/models")]
        public async Task<IActionResult> GetModelsByBrandId(int brandId)
        {
            try
            {
                var result = await _carAdService.GetModelsByBrandIdAsync(brandId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}