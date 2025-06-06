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
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            var categoryEntity = new Category
            {
                CategoryName = categoryDto.CategoryName
            };

            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = categoryEntity.CategoryId,
                CategoryName = categoryEntity.CategoryName
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);
            if (categoryEntity == null)
            {
                return false;
            }

            _context.Categories.Remove(categoryEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}