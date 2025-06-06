using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidGames.DTOs
{
    public class CreateGameDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Developer { get; set; }

        public DateTime ReleaseDate { get; set; }

        [StringLength(100)]
        public string? ImgNumber { get; set; }

        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}