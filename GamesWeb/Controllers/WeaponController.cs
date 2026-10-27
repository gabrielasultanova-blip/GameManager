using Game_Manager_Core.Interfaces;
using GameManager.Entities;
using GameManager.Migrations;
using GamesWeb.ViewModels.Weapon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GamesWeb.Controllers
{
    public class WeaponController : Controller
    {
        private readonly IWeaponService weaponService;
        private readonly ICharacterService characterService;

        public WeaponController(
            IWeaponService weaponService,
            ICharacterService characterService)
        {
            this.weaponService = weaponService;
            this.characterService = characterService;
        }

        public async Task<IActionResult> Index()
        {
            var weapons = await weaponService.GetAllWeaponsAsync();

            var model = new List<WeaponIndexViewModel>();

            foreach (var weapon in weapons)
            {
                model.Add(new WeaponIndexViewModel
                {
                    Id = weapon.Id,
                    Name = weapon.Name,
                    Type = weapon.Type,
                    Damage = weapon.Damage,
                    CharacterName = string.Join(", ",
                        weapon.CharacterWeapons.Select(cw =>
                            cw.Character.FirstName + " " +
                            cw.Character.LastName))
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var weapon =
                await weaponService.GetWeaponWithCharactersAsync(id);

            if (weapon == null)
            {
                return NotFound();
            }

            var model = new WeaponDetailsViewModel
            {
                Id = weapon.Id,
                Name = weapon.Name,
                Type = weapon.Type,
                Damage = weapon.Damage,
                CharacterName = string.Join(", ",
                    weapon.CharacterWeapons.Select(cw =>
                        cw.Character.FirstName + " " +
                        cw.Character.LastName))
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCharacters();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            WeaponCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var weapon = new Weapon
                {
                    Name = model.Name,
                    Type = model.Type,
                    Damage = model.Damage
                };

                foreach (var characterId in model.CharacterIds)
                {
                    weapon.CharacterWeapons.Add(
                        new CharacterWeapon
                        {
                            CharacterId = characterId
                        });
                }

                await weaponService.AddWeaponAsync(weapon);

                return RedirectToAction(nameof(Index));
            }

            await LoadCharacters(model.CharacterIds);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var weapon = await weaponService.GetWeaponByIdAsync(id);

            if (weapon == null)
            {
                return NotFound();
            }

            var model = new WeaponEditViewModel
            {
                Id = weapon.Id,
                Name = weapon.Name,
                Type = weapon.Type,
                Damage = weapon.Damage,
                CharacterIds = weapon.CharacterWeapons
                    .Select(cw => cw.CharacterId)
                    .ToList()
            };

            await LoadCharacters(model.CharacterIds);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id, WeaponEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var weapon =
                    await weaponService.GetWeaponByIdAsync(id);

                if (weapon == null)
                {
                    return NotFound();
                }

                weapon.Name = model.Name;
                weapon.Type = model.Type;
                weapon.Damage = model.Damage;

                await weaponService.UpdateWeaponAsync(
                    weapon, model.CharacterIds);

                return RedirectToAction(nameof(Index));
            }

            await LoadCharacters(model.CharacterIds);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var weapon = await weaponService.GetWeaponByIdAsync(id);

            if (weapon == null)
            {
                return NotFound();
            }

            var model = new WeaponDeleteViewModel
            {
                Id = weapon.Id,
                Name = weapon.Name,
                Type = weapon.Type
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await weaponService.DeleteWeaponAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCharacters(
            List<int> selectedCharacters = null)
        {
            var characters =
                await characterService.GetAllCharactersAsync();

            var options = characters.Select(c => new
            {
                c.Id,
                FullName = c.FirstName + " " + c.LastName
            }).ToList();

            ViewBag.Characters = new MultiSelectList(
                options,
                "Id",
                "FullName",
                selectedCharacters);
        }
    }
}
