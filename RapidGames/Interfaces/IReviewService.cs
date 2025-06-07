using RapidGames.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidGames.Interfaces
{
    public interface IReviewService
    {
        // base CRUD
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<ReviewDto?> CreateReviewAsync(CreateReviewDto reviewDto);
    }
}