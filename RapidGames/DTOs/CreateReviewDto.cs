using System.ComponentModel.DataAnnotations;

namespace RapidGames.DTOs
{
    public class CreateReviewDto
    {
        [StringLength(2000)]
        public string? ReviewText { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public int GameId { get; set; }
    }
}