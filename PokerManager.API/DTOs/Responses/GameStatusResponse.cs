namespace PokerManager.API.DTOs.Responses;

// (Opcional pero útil) Para ver cómo va la partida en tiempo real
public class GameStatusResponse
{
    public int GameId { get; set; }
    public DateTime DatePlayed { get; set; }
    public bool IsFinished { get; set; }
    public List<PlayerStatusResponse> Players { get; set; } = new();
}

public class PlayerStatusResponse
{
    public string Username { get; set; } = string.Empty;
    public int RebuyCount { get; set; }       // Cuántos enganches lleva
    public decimal CurrentInvestment { get; set; } // Cuánta plata ha puesto hasta ahora
}