using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GamesWeb.ViewModels.Character
{
    public class CharacterCreateViewModel
    {
        [Required(ErrorMessage = "Името на героя е задължително!")]
        [MaxLength(50, ErrorMessage = "Името на героя не може да бъде по-дълго от 50 символа!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилията на героя е задължителна!")]
        [MaxLength(50, ErrorMessage = "Фамилията на героя не може да бъде по-дълга от 50 символа!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ролята на героя е задължителна!")]
        [MaxLength(50, ErrorMessage = "Ролята на героя не може да бъде по-дълга от 50 символа!")]
        public string Role { get; set; }

        [Range(0, 150, ErrorMessage = "Възрастта трябва да бъде между 0 и 150 години!")]
        public int Age { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Моля, изберете игра!")]
        public int GameId { get; set; }
    }
}

