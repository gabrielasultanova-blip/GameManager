using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Entities
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Range(0, 150)]
        public int Age { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public ICollection<CharacterWeapon> CharacterWeapons
        { get; set; } = new List<CharacterWeapon>();
    }
}
