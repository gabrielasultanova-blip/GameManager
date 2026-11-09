using Game_Manager_Core.Interfaces;
using GameManager.Entities;
using GameManager.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Game_Manager_Core.Services
{
    public class GameService : IGameService
    {
        private readonly GameDbContext context_new;

        public GameService(GameDbContext context)
        {
            context_new = context;
        }
        public async Task AddGameAsync(Game game)
        {
            context_new.Games.Add(game);
            await context_new.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(int id)
        {
            var game = await context_new.Games
                .Include(g => g.Characters)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game != null)
            {
                var characterIds = game.Characters
                    .Select(c => c.Id)
                    .ToList();

                var characterWeapons = await context_new.CharacterWeapons
                    .Where(cw => characterIds.Contains(cw.CharacterId))
                    .ToListAsync();

                context_new.CharacterWeapons.RemoveRange(characterWeapons);
                context_new.Characters.RemoveRange(game.Characters);
                context_new.Games.Remove(game);

                await context_new.SaveChangesAsync();
            }
        }

        public async Task<List<Game>> GetAllGamesAsync(string? search = null, string? genre = null, string? sortOrder = null)
        {
            var query = context_new.Games.Select(g => g);

            query = query.Include(g => g.Characters);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(g =>
                    g.Title.Contains(search));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(g =>
                    g.Genre == genre);
            }

            switch (sortOrder)
            {
                case "year_asc":
                    query = query.OrderBy(g => g.ReleaseYear);
                    break;

                case "year_desc":
                    query = query.OrderByDescending(g => g.ReleaseYear);
                    break;

                default:
                    query = query.OrderByDescending(g => g.ReleaseYear);
                    break;
            }

            return await query.ToListAsync();
        }

        public async Task<List<string>> GetAllGenresAsync()
        {
            return await context_new.Games
                .Select(g => g.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await context_new.Games.FindAsync(id);
        }

        public async Task<Game?> GetGameDetailsAsync(int id)
        {
            return await context_new.Games
                .Include(g => g.Characters)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task UpdateGameAsync(Game game)
        {
            await context_new.SaveChangesAsync();
        }
    }
}
