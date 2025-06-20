﻿using Microsoft.EntityFrameworkCore;
using RapidGames.Data;
using RapidGames.DTOs;
using RapidGames.Interfaces;
using RapidGames.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidGames.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            // Fetch all games and map them to GameDto, including categories
            return await _context.Games
                .Include(g => g.CategoryGames).ThenInclude(cg => cg.Category)
                .Select(gameEntity => new GameDto
                {
                    GameId = gameEntity.GameId,
                    Title = gameEntity.Title,
                    Developer = gameEntity.Developer,
                    Categories = gameEntity.CategoryGames.Select(cg => new CategoryDto
                    {
                        CategoryId = cg.Category.CategoryId,
                        CategoryName = cg.Category.CategoryName
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            // Fetch a single game by ID and map it to GameDto, including categories
            return await _context.Games
                .Where(g => g.GameId == id)
                .Include(g => g.CategoryGames).ThenInclude(cg => cg.Category)
                .Select(gameEntity => new GameDto
                {
                    GameId = gameEntity.GameId,
                    Title = gameEntity.Title,
                    Developer = gameEntity.Developer,
                    Categories = gameEntity.CategoryGames.Select(cg => new CategoryDto
                    {
                        CategoryId = cg.Category.CategoryId,
                        CategoryName = cg.Category.CategoryName
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<GameDto> CreateGameAsync(CreateGameDto createGameDto)
        {
            
            var gameEntity = new Game
            {
                Title = createGameDto.Title,
                Developer = createGameDto.Developer,
                ReleaseDate = createGameDto.ReleaseDate,
                ImgNumber = createGameDto.ImgNumber
            };

            if (createGameDto.CategoryIds != null && createGameDto.CategoryIds.Any())
            {
                foreach (var catId in createGameDto.CategoryIds)
                {
                    gameEntity.CategoryGames.Add(new CategoryGames { CategoryId = catId });
                }
            }

            _context.Games.Add(gameEntity);
            await _context.SaveChangesAsync();

            return (await GetGameByIdAsync(gameEntity.GameId))!;
        }

        public async Task<GameDto?> UpdateGameAsync(int id, UpdateGameDto updateGameDto)
        {
            // Validate the input DTO and fetch the existing game entity
            var gameEntity = await _context.Games
                .Include(g => g.CategoryGames)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (gameEntity == null)
            {
                return null;
            }

            gameEntity.Title = updateGameDto.Title;
            gameEntity.Developer = updateGameDto.Developer;
            gameEntity.ReleaseDate = updateGameDto.ReleaseDate;
            gameEntity.ImgNumber = updateGameDto.ImgNumber;

            gameEntity.CategoryGames.Clear();
            if (updateGameDto.CategoryIds != null && updateGameDto.CategoryIds.Any())
            {
                foreach (var catId in updateGameDto.CategoryIds)
                {
                    gameEntity.CategoryGames.Add(new CategoryGames { CategoryId = catId });
                }
            }

            await _context.SaveChangesAsync();
            return await GetGameByIdAsync(id);
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            // Fetch the game entity by ID and remove it
            var gameEntity = await _context.Games.FindAsync(id);
            if (gameEntity == null)
            {
                return false;
            }

            _context.Games.Remove(gameEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<GameDto?> AddCategoryToGameAsync(int gameId, int categoryId)
        {
            // add category from game bridge table service
            var game = await _context.Games
                                     .Include(g => g.CategoryGames)
                                     .FirstOrDefaultAsync(g => g.GameId == gameId);

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);

            if (game == null || !categoryExists)
            {
                return null; 
            }

            var linkAlreadyExists = game.CategoryGames.Any(cg => cg.CategoryId == categoryId);
            if (linkAlreadyExists)
            {
                return await GetGameByIdAsync(gameId);
            }

            var newLink = new CategoryGames { GameId = gameId, CategoryId = categoryId };
            _context.CategoryGames.Add(newLink);
            await _context.SaveChangesAsync();
            return await GetGameByIdAsync(gameId);
        }

        public async Task<GameDto?> RemoveCategoryFromGameAsync(int gameId, int categoryId)
        {
            // remove category from game bridge table service
            var linkToRemove = await _context.CategoryGames
                                             .FirstOrDefaultAsync(cg => cg.GameId == gameId && cg.CategoryId == categoryId);

            if (linkToRemove == null)
            {
                return null; 
            }

            _context.CategoryGames.Remove(linkToRemove);
            await _context.SaveChangesAsync();

            
            return await GetGameByIdAsync(gameId);
        }
    }
}