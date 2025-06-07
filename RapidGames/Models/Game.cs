using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidGames.Models 
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Developer { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? ImgNumber { get; set; }
        //A review can be applied to review
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        // a category can be applied to multiple games

        public virtual ICollection<CategoryGames> CategoryGames { get; set; } = new List<CategoryGames>();
    }
}