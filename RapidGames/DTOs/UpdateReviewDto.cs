using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RapidGames.DTOs
{
    public class UpdateReviewDto
    {
        [StringLength(2000, ErrorMessage = "Review text cannot exceed 2000 characters.")]
        public string? ReviewText { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
