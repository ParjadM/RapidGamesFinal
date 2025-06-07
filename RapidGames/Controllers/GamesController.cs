using Microsoft.AspNetCore.Mvc;
using RapidGames.DTOs;
using RapidGames.Interfaces;
using System.Threading.Tasks;

namespace RapidGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }
        /// <summary>
        /// return list of all games
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }
        /// <summary>
        /// return list of games by ID
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }
        /// <summary>
        /// create a game
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto createGameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newGame = await _gameService.CreateGameAsync(createGameDto);
            return CreatedAtAction(nameof(GetGame), new { id = newGame.GameId }, newGame);
        }
        /// <summary>
        /// update a game by id
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameDto updateGameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedGame = await _gameService.UpdateGameAsync(id, updateGameDto);
            if (updatedGame == null)
            {
                return NotFound();
            }
            return Ok(updatedGame);
        }
        /// <summary>
        /// deleta a games by ID
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}