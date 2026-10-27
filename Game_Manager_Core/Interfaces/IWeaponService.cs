using GameManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Manager_Core.Interfaces
{
    public interface IWeaponService
    {
        Task<List<Weapon>> GetAllWeaponsAsync();
        Task<Weapon?> GetWeaponByIdAsync(int id);
        Task<Weapon?> GetWeaponWithCharactersAsync(int id);
        Task AddWeaponAsync(Weapon weapon);
        Task UpdateWeaponAsync(Weapon weapon, List<int> characterIds);
        Task DeleteWeaponAsync(int id);
    }
}
