using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PokerManager.API.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Username { get; set; } // El apodo único (ej. "Choco", "Flaco")

    [MaxLength(100)]
    public string? FullName { get; set; } // Nombre real (opcional)

    // Relación: Un usuario puede haber jugado muchas partidas
    [JsonIgnore] // Importante: Evita ciclos infinitos al enviar datos al frontend
    public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
}