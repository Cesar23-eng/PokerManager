using Microsoft.EntityFrameworkCore;
using PokerManager.API.Data;
using PokerManager.API.DTOs.Requests;
using PokerManager.API.DTOs.Responses; // Importante para los Response
using PokerManager.API.Entities;

namespace PokerManager.API.Services;

public class GameService
{
    private readonly AppDbContext _context;

    public GameService(AppDbContext context)
    {
        _context = context;
    }

    // 1. INICIAR PARTIDA
    public async Task<Game> CreateGameAsync(CreateGameRequest request)
    {
        var game = new Game
        {
            InitialBuyInCost = request.InitialBuyIn,
            RebuyCost = request.RebuyCost,
            DatePlayed = DateTime.Now,
            IsFinished = false
        };

        // Validar usuarios y agregarlos
        foreach (var userId in request.UserIds)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                // Inicializamos al jugador en la mesa
                game.Players.Add(new GamePlayer { UserId = userId });
            }
        }

        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    // 2. REGISTRAR ENGANCHE (REBUY)
    public async Task AddRebuyAsync(RebuyRequest request)
    {
        var player = await _context.GamePlayers
            .FirstOrDefaultAsync(gp => gp.GameId == request.GameId && gp.UserId == request.UserId);

        if (player == null) throw new Exception("Jugador o partida no encontrados. Verifica los IDs.");

        player.RebuyCount++; // Sumamos el enganche (+1)
        await _context.SaveChangesAsync();
    }

    // 3. FINALIZAR Y CALCULAR RESULTADOS (ALGORITMO DE PAGOS)
    public async Task<List<SettlementResponse>> CalculateSettlementAsync(FinishGameRequest request)
    {
        var game = await _context.Games
            .Include(g => g.Players)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == request.GameId);

        if (game == null) throw new Exception("Partida no encontrada.");

        // A. Actualizar con cuánto se retira cada uno (viene del frontend)
        foreach (var result in request.PlayerResults)
        {
            var player = game.Players.FirstOrDefault(p => p.UserId == result.UserId);
            if (player != null)
            {
                player.CashOutAmount = result.FinalCashAmount;
            }
        }

        game.IsFinished = true;
        await _context.SaveChangesAsync();

        // B. Calcular deudas (Algoritmo de Matching)
        var settlements = new List<SettlementResponse>();

        // Separar deudores (Perdieron dinero) y acreedores (Ganaron dinero)
        var debtors = game.Players
            .Where(p => p.NetResult < 0)
            .OrderBy(p => p.NetResult) // Los que perdieron más van primero
            .ToList();

        var creditors = game.Players
            .Where(p => p.NetResult > 0)
            .OrderByDescending(p => p.NetResult) // Los que ganaron más van primero
            .ToList();

        // Usamos diccionarios temporales para ir restando los montos mientras calculamos
        var debtBalances = debtors.ToDictionary(d => d.Id, d => Math.Abs(d.NetResult));
        var creditBalances = creditors.ToDictionary(c => c.Id, c => c.NetResult);

        int i = 0; // índice para recorrer deudores
        int j = 0; // índice para recorrer acreedores

        while (i < debtors.Count && j < creditors.Count)
        {
            var debtor = debtors[i];
            var creditor = creditors[j];

            // ¿Cuánto se puede pagar en esta transacción? Lo mínimo entre lo que debe uno y lo que le deben al otro.
            decimal amountToPay = Math.Min(debtBalances[debtor.Id], creditBalances[creditor.Id]);

            if (amountToPay > 0)
            {
                settlements.Add(new SettlementResponse
                {
                    DebtorName = debtor.User.Username,
                    CreditorName = creditor.User.Username,
                    Amount = Math.Round(amountToPay, 2)
                });
            }

            // Restamos lo pagado de los balances temporales
            debtBalances[debtor.Id] -= amountToPay;
            creditBalances[creditor.Id] -= amountToPay;

            // Si la deuda quedó en 0 (o casi 0 por decimales), pasamos al siguiente deudor
            if (debtBalances[debtor.Id] < 0.01m) i++;
            
            // Si el crédito quedó en 0, pasamos al siguiente acreedor
            if (creditBalances[creditor.Id] < 0.01m) j++;
        }

        return settlements;
    }
    
    // 4. OBTENER ESTADO ACTUAL (Opcional, para ver en vivo)
    public async Task<GameStatusResponse?> GetGameStatusAsync(int gameId)
    {
        var game = await _context.Games
            .Include(g => g.Players)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null) return null;

        return new GameStatusResponse
        {
            GameId = game.Id,
            DatePlayed = game.DatePlayed,
            IsFinished = game.IsFinished,
            Players = game.Players.Select(p => new PlayerStatusResponse
            {
                Username = p.User.Username,
                RebuyCount = p.RebuyCount,
                CurrentInvestment = p.TotalInvested // Propiedad calculada en Entity
            }).ToList()
        };
    }
}