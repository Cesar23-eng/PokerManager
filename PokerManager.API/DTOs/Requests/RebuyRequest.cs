namespace PokerManager.API.DTOs.Requests;

// Se envía cuando alguien pierde todo y pide fichas (se engancha)
public class RebuyRequest
{
    public int GameId { get; set; } // En qué partida está ocurriendo
    public int UserId { get; set; } // Quién es el que pide fichas
}