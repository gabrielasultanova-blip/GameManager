using Game_Manager_Core.Interfaces;
using GameManager.Entities;
using GameManager.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await context_new.Games.ToListAsync();
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
