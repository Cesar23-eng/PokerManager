namespace PokerManager.API.DTOs.Requests;

// Datos necesarios para registrar un nuevo amigo en la base de datos
public class CreateUserRequest
{
    public required string Username { get; set; } // Ej: "Choco"
    public string? FullName { get; set; }         // Ej: "Jorge Perez"
}