using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Entities
{
    public class Weapon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Range(1, 100)]
        public int Damage { get; set; }

        public ICollection<CharacterWeapon> CharacterWeapons
        { get; set; } = new List<CharacterWeapon>();
    }
}
