namespace PokerManager.API.DTOs.Requests;

// Datos finales: Con cuánto dinero se retira cada uno
public class FinishGameRequest
{
    public int GameId { get; set; }
    public List<PlayerFinalResult> PlayerResults { get; set; } = new();
}

// Clase auxiliar para saber cuánto tiene cada jugador específico
public class PlayerFinalResult
{
    public int UserId { get; set; }
    public decimal FinalCashAmount { get; set; } // Dinero (o valor en fichas) que tiene al final
}