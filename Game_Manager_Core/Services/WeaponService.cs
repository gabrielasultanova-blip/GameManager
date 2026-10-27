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
    public class WeaponService : IWeaponService
    {
        private readonly GameDbContext context_new;

        public WeaponService(GameDbContext context)
        {
            context_new = context;
        }
        public async Task AddWeaponAsync(Weapon weapon)
        {
            context_new.Weapons.Add(weapon);
            await context_new.SaveChangesAsync();
        }

        public async Task DeleteWeaponAsync(int id)
        {
            var weapon = await context_new.Weapons
                .Include(w => w.CharacterWeapons)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (weapon != null)
            {
                context_new.CharacterWeapons.RemoveRange(
                    weapon.CharacterWeapons);

                context_new.Weapons.Remove(weapon);

                await context_new.SaveChangesAsync();
            }
        }

        public async Task<List<Weapon>> GetAllWeaponsAsync()
        {
            return await context_new.Weapons
                .Include(w => w.CharacterWeapons)
                .ThenInclude(cw => cw.Character)
                .ToListAsync();
        }

        public async Task<Weapon?> GetWeaponByIdAsync(int id)
        {
            return await context_new.Weapons
                .Include(w => w.CharacterWeapons)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Weapon?> GetWeaponWithCharactersAsync(int id)
        {
            return await context_new.Weapons
                .Include(w => w.CharacterWeapons)
                .ThenInclude(cw => cw.Character)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task UpdateWeaponAsync(Weapon weapon, List<int> characterIds)
        {
            var selectedIds = characterIds.Distinct().ToList();

            var oldRelations = weapon.CharacterWeapons
                .Where(cw => !selectedIds.Contains(cw.CharacterId))
                .ToList();

            context_new.CharacterWeapons.RemoveRange(oldRelations);

            var existingIds = weapon.CharacterWeapons
                .Select(cw => cw.CharacterId)
                .ToList();

            foreach (var characterId in selectedIds)
            {
                if (!existingIds.Contains(characterId))
                {
                    weapon.CharacterWeapons.Add(new CharacterWeapon
                    {
                        CharacterId = characterId,
                        WeaponId = weapon.Id
                    });
                }
            }

            await context_new.SaveChangesAsync();
        }
    }
}
