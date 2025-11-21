using Microsoft.EntityFrameworkCore;
using PokerManager.API.Entities;

namespace PokerManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Aquí declaramos qué tablas queremos en la BD
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GamePlayer> GamePlayers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración explícita de relaciones (Buenas prácticas)
        
        // Un GamePlayer tiene 1 Juego y 1 Usuario
        modelBuilder.Entity<GamePlayer>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.Players)
            .HasForeignKey(gp => gp.GameId)
            .OnDelete(DeleteBehavior.Cascade); // Si borro el juego, se borran los registros de jugadores de ese juego

        modelBuilder.Entity<GamePlayer>()
            .HasOne(gp => gp.User)
            .WithMany(u => u.GamePlayers)
            .HasForeignKey(gp => gp.UserId)
            .OnDelete(DeleteBehavior.Restrict); // IMPORTANTE: Si intento borrar un Usuario que ya jugó, NO me deja (para no romper el historial)
            
        // Hacer que el Username sea único en la BD
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}