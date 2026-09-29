using System.ComponentModel.DataAnnotations;

namespace GamesWeb.ViewModels.Game
{
    public class GameEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на играта е задължително!")]
        [MaxLength(100, ErrorMessage = "Името на играта не може да бъде по-дълго от 100 символа!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Жанрът е задължителен!")]
        [MaxLength(50, ErrorMessage = "Жанрът не може да бъде по-дълъг от 50 символа!")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Годината на издаване е задължителна!")]
        [Range(1950, 2100, ErrorMessage = "Годината на издаване трябва да бъде между 1950 и 2100!")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Възрастовото ограничение е задължително!")]
        [MaxLength(20, ErrorMessage = "Възрастовото ограничение не може да бъде по-дълго от 20 символа!")]
        public string AgeRating { get; set; }

        [Required(ErrorMessage = "Разработчикът е задължителен!")]
        [MaxLength(100, ErrorMessage = "Името на разработчика не може да бъде по-дълго от 100 символа!")]
        public string Developer { get; set; }
    }
}
