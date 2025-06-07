using Microsoft.AspNetCore.Mvc;
using RapidGames.DTOs;
using RapidGames.Interfaces;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    /// <summary>
    /// return all reviews
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetReviews()
    {
        var reviews = await _reviewService.GetAllReviewsAsync();
        return Ok(reviews);
    }
    /// <summary>
    /// create a review
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newReview =await _reviewService.CreateReviewAsync(createReviewDto);
        if (newReview == null)
        {
            return BadRequest(new { message = "Invalid GameId" });
        }
        return CreatedAtAction(nameof(GetReviews), new { id = newReview.ReviewId }, newReview);
    }
}