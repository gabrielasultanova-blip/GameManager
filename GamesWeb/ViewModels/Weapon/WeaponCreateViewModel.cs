using System.ComponentModel.DataAnnotations;

namespace GamesWeb.ViewModels.Weapon
{
    public class WeaponCreateViewModel
    {
        [Required(ErrorMessage = "Името е задължително!")]
        [MaxLength(100, ErrorMessage = "Името не може да бъде по-дълго от 100 символа!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Типът е задължителен!")]
        [MaxLength(50, ErrorMessage = "Типът не може да бъде по-дълъг от 50 символа!")]
        public string Type { get; set; }

        [Range(1, 100, ErrorMessage = "Щетите трябва да бъдат между 1 и 100!")]
        public int Damage { get; set; }

        [Required(ErrorMessage = "Моля, изберете персонаж!")]
        public List<int> CharacterIds { get; set; } = new List<int>();
    }
}
