using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PokerManager.API.Entities;

public class GamePlayer
{
    [Key]
    public int Id { get; set; }

    // Claves foráneas (Foreign Keys)
    public int GameId { get; set; }
    [JsonIgnore]
    public Game Game { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // DATOS CLAVE PARA EL CÁLCULO
    public int RebuyCount { get; set; } = 0; // Cantidad de veces que se enganchó
    
    public decimal CashOutAmount { get; set; } = 0; // Con cuánto dinero terminó en la mano

    // Propiedades calculadas (No se guardan en BD, pero ayudan en el código)
    // Total invertido = Entrada + (Enganches * Costo Enganche)
    [NotMapped]
    public decimal TotalInvested => Game != null 
        ? Game.InitialBuyInCost + (RebuyCount * Game.RebuyCost) 
        : 0;

    // Resultado Neto = Lo que sacó - Lo que puso
    [NotMapped]
    public decimal NetResult => CashOutAmount - TotalInvested;
}