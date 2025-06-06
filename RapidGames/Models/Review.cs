using System.ComponentModel.DataAnnotations;

namespace RapidGames.Models 
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public int GameId { get; set; }
        public virtual Game? Game { get; set; }
    }
}