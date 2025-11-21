namespace PokerManager.API.DTOs.Requests;

// Datos para configurar la mesa al inicio de la noche
public class CreateGameRequest
{
    public decimal InitialBuyIn { get; set; }     // Costo de la entrada (Ej: 50)
    public decimal RebuyCost { get; set; }        // Costo del enganche (Ej: 50)
    public List<int> UserIds { get; set; } = new(); // Lista de IDs de los amigos que van a jugar
}