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
    public class CharacterService : ICharacterService
    {
        private readonly GameDbContext context_new;

        public CharacterService(GameDbContext context)
        {
            context_new = context;
        }
        public async Task AddCharacterAsync(Character character)
        {
            context_new.Characters.Add(character);
            await context_new.SaveChangesAsync();
        }

        public async Task DeleteCharacterAsync(int id)
        {
            var character = await context_new.Characters
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character != null)
            {
                var characterWeapons = await context_new.CharacterWeapons
                    .Where(cw => cw.CharacterId == id)
                    .ToListAsync();

                context_new.CharacterWeapons.RemoveRange(characterWeapons);
                context_new.Characters.Remove(character);

                await context_new.SaveChangesAsync();
            }
        }

        public async Task<List<Character>> GetAllCharactersAsync()
        {
            return await context_new.Characters
                .Include(c => c.Game)
                .ToListAsync();
        }

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            return await context_new.Characters.FindAsync(id);
        }

        public async Task<Character?> GetCharacterDetailsAsync(int id)
        {
            return await context_new.Characters
                .Include(c => c.Game)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCharacterAsync(Character character)
        {
            await context_new.SaveChangesAsync();
        }
    }
}
