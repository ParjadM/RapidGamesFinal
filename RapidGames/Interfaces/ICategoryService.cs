using RapidGames.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidGames.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}