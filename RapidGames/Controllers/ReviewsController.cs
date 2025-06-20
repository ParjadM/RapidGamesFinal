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
    /// <summary>
    /// Update Review
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto updateReviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var updatedReview = await _reviewService.UpdateReviewAsync(id, updateReviewDto);
        if (updatedReview == null)
        {
            return NotFound();
        }
        return Ok(updatedReview);
    }

    /// <summary>
    /// Delete Review
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var success = await _reviewService.DeleteReviewAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}