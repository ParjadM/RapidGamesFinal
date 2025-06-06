using System.ComponentModel.DataAnnotations;

namespace RapidGames.Models 
{
    public class CategoryGames
    {
        [Key]
        public int CategoryGamesId { get; set; } 
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int GameId { get; set; }
        public virtual Game? Game { get; set; }
    }
}