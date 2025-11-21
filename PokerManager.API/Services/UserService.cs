using Microsoft.EntityFrameworkCore;
using PokerManager.API.Data;
using PokerManager.API.DTOs.Requests; // Nuevo namespace
using PokerManager.API.Entities;

namespace PokerManager.API.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(CreateUserRequest request)
    {
        // Validación simple: no permitir nombres vacíos
        if (string.IsNullOrWhiteSpace(request.Username)) 
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        var user = new User 
        { 
            Username = request.Username, 
            FullName = request.FullName 
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}