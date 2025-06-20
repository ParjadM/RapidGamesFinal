using RapidGames.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidGames.Interfaces
{
    public interface IGameService
    {
        // base CRUD
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<GameDto?> GetGameByIdAsync(int id);
        Task<GameDto> CreateGameAsync(CreateGameDto createGameDto);
        Task<GameDto?> UpdateGameAsync(int id, UpdateGameDto updateGameDto);
        Task<bool> DeleteGameAsync(int id);


        // bridge table
        Task<GameDto?> AddCategoryToGameAsync(int gameId, int categoryId);
        Task<GameDto?> RemoveCategoryFromGameAsync(int gameId, int categoryId);
    }
}