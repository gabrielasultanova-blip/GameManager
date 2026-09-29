using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } 

        [Range(1950, 2100)]
        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(20)]
        public string AgeRating { get; set; } 

        [Required]
        [StringLength(100)]
        public string Developer { get; set; } 

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}

