using CarAuctionApi.Data;
using CarAuctionApi.Models.Dtos;
using CarAuctionApi.Models.Entities;
using CarAuctionApi.Repositories.Interfaces;
using CarAuctionApi.Services.Interfaces;

namespace CarAuctionApi.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly ICarAdRepository _carAdRepository;
        private readonly IDownPaymentRepository _downPaymentRepository;
        private readonly ApplicationDbContext _context;
        private const decimal DownPaymentAmount = 3000m;
        private const decimal MinimumBidIncrement = 1000m;

        public BidService(IBidRepository bidRepository, ICarAdRepository carAdRepository, IDownPaymentRepository downPaymentRepository, ApplicationDbContext context)
        {
            _bidRepository = bidRepository;
            _carAdRepository = carAdRepository;
            _downPaymentRepository = downPaymentRepository;
            _context = context;
        }

        public async Task<BidDto> PlaceBidAsync(BidCreateDto dto, string userId)
        {
            var carAd = await _carAdRepository.GetByIdAsync(dto.CarAdId);
            if (carAd == null)
            {
                throw new Exception("Car ad not found");
            }

            if (!carAd.IsBiddable)
            {
                throw new Exception("This car ad is not open for bidding");
            }

            if (carAd.AuctionEndDate < DateTime.UtcNow)
            {
                throw new Exception("Auction has ended");
            }

            if (carAd.UserId == userId)
            {
                throw new Exception("You cannot bid on your own car ad");
            }

            var highestBid = await _bidRepository.GetHighestBidAmountAsync(dto.CarAdId) ?? 0m;
            if (dto.Amount <= highestBid + MinimumBidIncrement)
            {
                throw new Exception($"Bid must be at least {highestBid + MinimumBidIncrement} SEK");
            }

            var bid = new Bid
            {
                Id = Guid.NewGuid(),
                CarAdId = dto.CarAdId,
                UserId = userId,
                Amount = dto.Amount
            };

            await _bidRepository.CreateAsync(bid);

            // Update CurrentHighestBid
            carAd.CurrentHighestBid = dto.Amount;
            await _carAdRepository.UpdateAsync(carAd);

            var user = await _context.Users.FindAsync(userId);
            return new BidDto
            {
                Id = bid.Id,
                CarAdId = bid.CarAdId,
                UserId = userId,
                UserName = $"{user?.FirstName} {user?.LastName}",
                Amount = bid.Amount,
                CreatedAt = bid.CreatedAt
            };
        }

        public async Task<List<BidDto>> GetBidsByCarAdIdAsync(Guid carAdId)
        {
            var bids = await _bidRepository.GetByCarAdIdAsync(carAdId);
            return bids.Select(b => new BidDto
            {
                Id = b.Id,
                CarAdId = b.CarAdId,
                UserId = b.UserId,
                UserName = $"{b.User?.FirstName} {b.User?.LastName}",
                Amount = b.Amount,
                CreatedAt = b.CreatedAt
            }).ToList();
        }

        public async Task<DownPaymentDto> BuyFixedPriceAsync(Guid carAdId, string buyerId)
        {
            var carAd = await _carAdRepository.GetByIdAsync(carAdId);
            if (carAd == null)
            {
                throw new Exception("Car ad not found");
            }

            if (!carAd.FixedPrice.HasValue)
            {
                throw new Exception("This car ad does not have a fixed price");
            }

            if (carAd.IsSold)
            {
                throw new Exception("This car ad is already sold");
            }

            if (carAd.UserId == buyerId)
            {
                throw new Exception("You cannot buy your own car ad");
            }

            var existingDownPayment = await _downPaymentRepository.GetByCarAdIdAndBuyerIdAsync(carAdId, buyerId);
            if (existingDownPayment != null)
            {
                throw new Exception("You have already made a down payment for this car ad");
            }

            var downPayment = new DownPayment
            {
                Id = Guid.NewGuid(),
                CarAdId = carAdId,
                BuyerId = buyerId,
                Amount = DownPaymentAmount,
                IsForfeited = false
            };

            await _downPaymentRepository.CreateAsync(downPayment);

            // Mark car ad as sold and record down payment
            carAd.IsSold = true;
            carAd.BuyerId = buyerId;
            carAd.HasDownPayment = true;
            await _carAdRepository.UpdateAsync(carAd);

            var buyer = await _context.Users.FindAsync(buyerId);
            return new DownPaymentDto
            {
                Id = downPayment.Id,
                CarAdId = downPayment.CarAdId,
                BuyerId = buyerId,
                BuyerName = $"{buyer?.FirstName} {buyer?.LastName}",
                Amount = downPayment.Amount,
                IsForfeited = downPayment.IsForfeited,
                CreatedAt = downPayment.CreatedAt
            };
        }
    }
}