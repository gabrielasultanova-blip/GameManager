using GameManager.Entities;
using GameManager.Migrations;
using GamesWeb.ViewModels.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamesWeb.Controllers
{
    public class GameController : Controller
    {
        private readonly GameDbContext context;

        public GameController(GameDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var games = await context.Games
                .ToListAsync();

            var model = new List<GameIndexViewModel>();

            foreach (var game in games)
            {
                model.Add(new GameIndexViewModel
                {
                    Id = game.Id,
                    Title = game.Title,
                    Genre = game.Genre,
                    ReleaseYear = game.ReleaseYear,
                    AgeRating = game.AgeRating,
                    Developer = game.Developer
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var game = await context.Games
                .Include(g => g.Characters)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameDetailsViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Genre = game.Genre,
                ReleaseYear = game.ReleaseYear,
                AgeRating = game.AgeRating,
                Developer = game.Developer,
                Characters = game.Characters
                    .Select(c => c.FirstName + " " + c.LastName)
                    .ToList()
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    Title = model.Title,
                    Genre = model.Genre,
                    ReleaseYear = model.ReleaseYear,
                    AgeRating = model.AgeRating,
                    Developer = model.Developer
                };

                context.Games.Add(game);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameEditViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Genre = game.Genre,
                ReleaseYear = game.ReleaseYear,
                AgeRating = game.AgeRating,
                Developer = game.Developer
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GameEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var game = await context.Games.FindAsync(id);

                if (game == null)
                {
                    return NotFound();
                }

                game.Title = model.Title;
                game.Genre = model.Genre;
                game.ReleaseYear = model.ReleaseYear;
                game.AgeRating = model.AgeRating;
                game.Developer = model.Developer;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var game = await context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameDeleteViewModel
            {
                Id = game.Id,
                Title = game.Title
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await context.Games.FindAsync(id);

            if (game != null)
            {
                context.Games.Remove(game);

                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
