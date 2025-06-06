using Microsoft.EntityFrameworkCore;
using RapidGames.Data;
using RapidGames.DTOs;
using RapidGames.Interfaces;
using RapidGames.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidGames.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Game) 
                .Select(r => new ReviewDto
                {
                    ReviewId = r.ReviewId,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    GameId = r.GameId,
                    GameTitle = r.Game.Title
                })
                .ToListAsync();
        }

        public async Task<ReviewDto?> CreateReviewAsync(CreateReviewDto reviewDto)
        {
            var gameExists = await _context.Games.AnyAsync(g => g.GameId == reviewDto.GameId);
            if (!gameExists)
            {
                return null; 
            }

            var reviewEntity = new Review
            {
                ReviewText = reviewDto.ReviewText,
                Rating = reviewDto.Rating,
                GameId = reviewDto.GameId
            };

            _context.Reviews.Add(reviewEntity);
            await _context.SaveChangesAsync();

            var newReview = await _context.Reviews.Include(r => r.Game).FirstOrDefaultAsync(r => r.ReviewId == reviewEntity.ReviewId);

            return new ReviewDto
            {
                ReviewId = newReview.ReviewId,
                ReviewText = newReview.ReviewText,
                Rating = newReview.Rating,
                GameId = newReview.GameId,
                GameTitle = newReview.Game.Title
            };
        }
    }
}