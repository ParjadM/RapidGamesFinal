namespace RapidGames.DTOs
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public int GameId { get; set; }
        public string? GameTitle { get; set; }
    }
}