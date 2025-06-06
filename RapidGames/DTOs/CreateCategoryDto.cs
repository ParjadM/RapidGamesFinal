using System.ComponentModel.DataAnnotations;

namespace RapidGames.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;
    }
}