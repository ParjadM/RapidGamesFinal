using Microsoft.AspNetCore.Mvc;
using RapidGames.DTOs;
using RapidGames.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService =categoryService;
    }
    /// <summary>
    /// Returns a list of Categories
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }
    /// <summary>
    /// Returns a category by its ID
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category =await _categoryService.GetCategoryByIdAsync(id);
        if (category== null)
        {
            return NotFound();
        }
        return Ok(category);
    }
    /// <summary>
    /// create a category 
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);
        return CreatedAtAction(nameof(GetCategory), new { id = newCategory.CategoryId }, newCategory);
    }
    /// <summary>
    /// delete a category by its ID
    /// </summary>
    /// <returns>
    /// 200 OK
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var success =await _categoryService.DeleteCategoryAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}