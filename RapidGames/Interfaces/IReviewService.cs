using RapidGames.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidGames.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<ReviewDto?> CreateReviewAsync(CreateReviewDto reviewDto);
    }
}