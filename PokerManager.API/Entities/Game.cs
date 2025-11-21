using System.ComponentModel.DataAnnotations;

namespace PokerManager.API.Entities;

public class Game
{
    [Key]
    public int Id { get; set; }

    public DateTime DatePlayed { get; set; } = DateTime.Now;

    public bool IsFinished { get; set; } = false; // Para saber si ya se cerró la cuenta

    // Configuración de dinero de ESA noche
    [Range(0, double.MaxValue)]
    public decimal InitialBuyInCost { get; set; } // Cuánto cuesta sentarse a jugar

    [Range(0, double.MaxValue)]
    public decimal RebuyCost { get; set; } // Cuánto cuesta cada enganche

    // Relación: Una partida tiene muchos jugadores
    public ICollection<GamePlayer> Players { get; set; } = new List<GamePlayer>();
}