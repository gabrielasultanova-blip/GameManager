using GameManager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
namespace GameManager.Migrations
{
    public class GameDbContext: DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
          : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }  
        public DbSet<Character> Characters { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<CharacterWeapon> CharacterWeapons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CharacterWeapon>()
                .HasKey(cw => new { cw.CharacterId, cw.WeaponId });

            modelBuilder.Entity<CharacterWeapon>()
                .HasOne(cw => cw.Character)
                .WithMany(c => c.CharacterWeapons)
                .HasForeignKey(cw => cw.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharacterWeapon>()
                .HasOne(cw => cw.Weapon)
                .WithMany(w => w.CharacterWeapons)
                .HasForeignKey(cw => cw.WeaponId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
