using Game_Manager_Core.Interfaces;
using GameManager.Entities;
using GameManager.Migrations;
using GamesWeb.ViewModels.Character;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GamesWeb.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ICharacterService characterService;
        private readonly IGameService gameService;

        public CharacterController(
            ICharacterService characterService,
            IGameService gameService)
        {
            this.characterService = characterService;
            this.gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var characters =
                await characterService.GetAllCharactersAsync();

            var model = new List<CharacterIndexViewModel>();

            foreach (var character in characters)
            {
                model.Add(new CharacterIndexViewModel
                {
                    Id = character.Id,
                    FirstName = character.FirstName,
                    LastName = character.LastName,
                    Role = character.Role,
                    Age = character.Age,
                    GameTitle = character.Game.Title
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var character =
                await characterService.GetCharacterDetailsAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            var model = new CharacterDetailsViewModel
            {
                Id = character.Id,
                FirstName = character.FirstName,
                LastName = character.LastName,
                Role = character.Role,
                Age = character.Age,
                GameTitle = character.Game.Title
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Games = new SelectList(
                await gameService.GetAllGamesAsync(),
                "Id",
                "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CharacterCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var character = new Character
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,
                    Age = model.Age,
                    GameId = model.GameId
                };

                await characterService.AddCharacterAsync(character);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Games = new SelectList(
                await gameService.GetAllGamesAsync(),
                "Id",
                "Title");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var character =
                await characterService.GetCharacterByIdAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            var model = new CharacterEditViewModel
            {
                Id = character.Id,
                FirstName = character.FirstName,
                LastName = character.LastName,
                Role = character.Role,
                Age = character.Age,
                GameId = character.GameId
            };

            ViewBag.Games = new SelectList(
                await gameService.GetAllGamesAsync(),
                "Id",
                "Title");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id, CharacterEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var character =
                    await characterService.GetCharacterByIdAsync(id);

                if (character == null)
                {
                    return NotFound();
                }

                character.FirstName = model.FirstName;
                character.LastName = model.LastName;
                character.Role = model.Role;
                character.Age = model.Age;
                character.GameId = model.GameId;

                await characterService.UpdateCharacterAsync(character);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Games = new SelectList(
                await gameService.GetAllGamesAsync(),
                "Id",
                "Title");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var character =
                await characterService.GetCharacterByIdAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            var model = new CharacterDeleteViewModel
            {
                Id = character.Id,
                FirstName = character.FirstName,
                LastName = character.LastName
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await characterService.DeleteCharacterAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
