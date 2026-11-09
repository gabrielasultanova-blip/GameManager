using GameManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Manager_Core.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetAllGamesAsync(
            string? search = null,
            string? genre = null,
            string? sortOrder = null);

        Task<List<string>> GetAllGenresAsync();

        Task<Game?> GetGameByIdAsync(int id);
        Task<Game?> GetGameDetailsAsync(int id);
        Task AddGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(int id);
    }
}
