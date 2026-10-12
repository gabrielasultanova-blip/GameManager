using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Entities
{
    public class CharacterWeapon
    {
        public int CharacterId { get; set; }
        public int WeaponId { get; set; }

        public Character Character { get; set; }
        public Weapon Weapon { get; set; }
    }
}
