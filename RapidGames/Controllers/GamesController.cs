using Microsoft.AspNetCore.Mvc;
using RapidGames.DTOs;
using RapidGames.Interfaces;
using RapidGames.Models;
using RapidGames.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RapidGames.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ApplicationDbContext _context;

        public GamesController(IGameService gameService, ApplicationDbContext context)
        {
            _gameService = gameService;
            _context = context;
        }

        /// <summary>
        /// Returns a view with a list of all games.
        /// </summary>
        /// <returns>A view containing the list of games.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Games.ToListAsync());
        }

        /// <summary>
        /// Returns a view with the details of a specific game.
        /// </summary>
        /// <param name="id">The ID of the game to retrieve.</param>
        /// <returns>A view displaying the game's details.</returns>
        public async Task<IActionResult> Details(int? id)
        {

            var game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == id);
            return View(game);
        }

        /// <summary>
        /// Returns the view with the form for creating a new game.
        /// </summary>
        /// <returns>The create game view.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the form submission for creating a new game.
        /// </summary>
        /// <param name="game">The game object created from the form data.</param>
        /// <returns>Redirects to the Index view if successful, otherwise returns the Create view with errors.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([Bind("GameId,Title,Developer,ImgNumber")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        /// <summary>
        /// Returns the view with the form for editing an existing game.
        /// </summary>
        /// <param name="id">The ID of the game to edit.</param>
        /// <returns>The edit game view with the game's current data.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            var game = await _context.Games.FindAsync(id);
            return View(game);
        }

        /// <summary>
        /// Handles the form submission for updating an existing game.
        /// </summary>
        /// <param name="id">The ID of the game being edited.</param>
        /// <param name="game">The game object with the updated data from the form.</param>
        /// <returns>Redirects to the Index view</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Title,Developer,ImgNumber")] Game game)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        /// <summary>
        /// Handles the deletion of a game.
        /// </summary>
        /// <param name="id">The ID of the game to delete.</param>
        /// <returns>Redirects to the Index view.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
        /// <summary>
        /// Returns a list of all games via API.
        /// </summary>
        /// <returns>200 OK with a list of games.</returns>
        [HttpGet("api/games")]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        /// <summary>
        /// Returns a specific game by ID via API.
        /// </summary>
        /// <param name="id">The ID of the game to retrieve.</param>
        /// <returns>200 OK with the game data, or 404 Not Found.</returns>
        [HttpGet("api/games/{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            return Ok(game);
        }

        /// <summary>
        /// Creates a new game via API.
        /// </summary>
        /// <param name="createGameDto">The data for the new game.</param>
        /// <returns>201 Created with the new game data.</returns>
        [HttpPost("api/games")]
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
        /// Updates an existing game by ID via API.
        /// </summary>
        /// <param name="id">The ID of the game to update.</param>
        /// <param name="updateGameDto">The updated data for the game.</param>
        /// <returns>200 OK with the updated game data, or 404 Not Found.</returns>
        [HttpPut("api/games/{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameDto updateGameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedGame = await _gameService.UpdateGameAsync(id, updateGameDto);
            return Ok(updatedGame);
        }

        /// <summary>
        /// Deletes a game by ID via API.
        /// </summary>
        /// <param name="id">The ID of the game to delete.</param>
        /// <returns>204 No Content if successful, or 404 Not Found.</returns>
        [HttpDelete("api/games/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Adds a category to a game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="categoryId">The ID of the category to add.</param>
        /// <returns>200 OK with the updated game data.</returns>
        [HttpPost("api/games/{gameId}/categories/{categoryId}")]
        public async Task<IActionResult> AddCategoryToGame(int gameId, int categoryId)
        {
            var updatedGame = await _gameService.AddCategoryToGameAsync(gameId, categoryId);
            return Ok(updatedGame);
        }

        /// <summary>
        /// Removes a category from a game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="categoryId">The ID of the category to remove.</param>
        /// <returns>200 OK with the updated game data.</returns>
        [HttpDelete("api/games/{gameId}/categories/{categoryId}")]
        public async Task<IActionResult> RemoveCategoryFromGame(int gameId, int categoryId)
        {
            var updatedGame = await _gameService.RemoveCategoryFromGameAsync(gameId, categoryId);
            return Ok(updatedGame);
        }
    }
}
