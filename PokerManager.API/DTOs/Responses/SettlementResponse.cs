namespace PokerManager.API.DTOs.Responses;

// La respuesta final: Quién le paga a quién y cuánto
public class SettlementResponse
{
    public string DebtorName { get; set; } = string.Empty;   // El que paga (Perdió)
    public string CreditorName { get; set; } = string.Empty; // El que cobra (Ganó)
    public decimal Amount { get; set; }                      // Cuánto debe pagar
}