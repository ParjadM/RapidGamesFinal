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
                    GameTitle = r.Game != null ? r.Game.Title : "Unknown Game"
                })
                .ToListAsync();
        }

        public async Task<ReviewDto?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .Where(r => r.ReviewId == id)
                .Include(r => r.Game)
                .Select(r => new ReviewDto
                {
                    ReviewId = r.ReviewId,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    GameId = r.GameId,
                    GameTitle = r.Game != null ? r.Game.Title : "Unknown Game"
                })
                .FirstOrDefaultAsync();
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

            return await GetReviewByIdAsync(reviewEntity.ReviewId);
        }

        public async Task<ReviewDto?> UpdateReviewAsync(int id, UpdateReviewDto reviewDto)
        {
            var reviewEntity = await _context.Reviews.FindAsync(id);

            if (reviewEntity == null)
            {
                return null; 
            }

            reviewEntity.ReviewText = reviewDto.ReviewText;
            reviewEntity.Rating = reviewDto.Rating;

            await _context.SaveChangesAsync();

            return await GetReviewByIdAsync(id);
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var reviewEntity = await _context.Reviews.FindAsync(id);
            if (reviewEntity == null)
            {
                return false; 
            }

            _context.Reviews.Remove(reviewEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}