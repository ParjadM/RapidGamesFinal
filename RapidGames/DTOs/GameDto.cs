namespace RapidGames.DTOs
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Developer { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}