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
        private readonly GameDbContext context;

        public CharacterController(GameDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var characters = await context.Characters
                .Include(c => c.Game)
                .ToListAsync();

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
            var character = await context.Characters
                .Include(c => c.Game)
                .FirstOrDefaultAsync(c => c.Id == id);

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
                await context.Games.ToListAsync(),
                "Id",
                "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CharacterCreateViewModel model)
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

                context.Characters.Add(character);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Games = new SelectList(
                await context.Games.ToListAsync(),
                "Id",
                "Title");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var character = await context.Characters.FindAsync(id);

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
                await context.Games.ToListAsync(),
                "Id",
                "Title");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CharacterEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var character = await context.Characters.FindAsync(id);

                if (character == null)
                {
                    return NotFound();
                }

                character.FirstName = model.FirstName;
                character.LastName = model.LastName;
                character.Role = model.Role;
                character.Age = model.Age;
                character.GameId = model.GameId;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Games = new SelectList(
                await context.Games.ToListAsync(),
                "Id",
                "Title");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var character = await context.Characters.FindAsync(id);

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
            var character = await context.Characters.FindAsync(id);

            if (character != null)
            {
                context.Characters.Remove(character);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
