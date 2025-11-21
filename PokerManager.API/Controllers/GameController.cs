using Microsoft.AspNetCore.Mvc;
using PokerManager.API.DTOs.Requests;
using PokerManager.API.DTOs.Responses;
using PokerManager.API.Services;

namespace PokerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    // POST: api/game/start
    [HttpPost("start")]
    public async Task<IActionResult> StartGame([FromBody] CreateGameRequest request)
    {
        try
        {
            var game = await _gameService.CreateGameAsync(request);
            return Ok(new { GameId = game.Id, Message = "¡Partida iniciada! A jugar." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    // POST: api/game/rebuy
    [HttpPost("rebuy")]
    public async Task<IActionResult> AddRebuy([FromBody] RebuyRequest request)
    {
        try
        {
            await _gameService.AddRebuyAsync(request);
            return Ok(new { Message = "Enganche registrado correctamente. +1 deuda." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    // POST: api/game/finish
    [HttpPost("finish")]
    public async Task<ActionResult<List<SettlementResponse>>> FinishGame([FromBody] FinishGameRequest request)
    {
        try
        {
            var settlements = await _gameService.CalculateSettlementAsync(request);
            return Ok(settlements); // Devuelve la lista de quién paga a quién
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    // GET: api/game/{id}/status
    [HttpGet("{id}/status")]
    public async Task<ActionResult<GameStatusResponse>> GetStatus(int id)
    {
        var status = await _gameService.GetGameStatusAsync(id);
        if (status == null) return NotFound("Partida no encontrada.");
        return Ok(status);
    }
}