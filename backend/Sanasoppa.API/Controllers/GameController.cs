using System.ComponentModel;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Core.Services;

namespace Sanasoppa.API.Controllers;

public class GameController : ApiBaseController
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<GameSessionDto>), Description = "Returns all game sessions")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, null, Description = "Internal server error")]
    [OpenApiOperation("GetGameSessions")]
    [Description("Returns all game sessions")]
    public async Task<ActionResult<GameSessionDto>> GetGameSessionsAsync()
    {
        var gameSessions = await _gameService.GetGameSessionsAsync();
        return Ok(gameSessions);
    }

    [HttpGet("{id}")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(GameSessionDto), Description = "Returns a game session by id")]
    [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "Game session not found")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, null, Description = "Internal server error")]
    [OpenApiOperation("GetGameSessionById")]
    [Description("Returns a game session by id")]
    public async Task<ActionResult<GameSessionDto>> GetGameSessionByIdAsync(Guid id)
    {
        try
        {
            var gameSession = await _gameService.GetGameSessionByIdAsync(id);
            return Ok(gameSession);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [SwaggerResponse(HttpStatusCode.Created, typeof(GameSessionDto), Description = "Creates a new game session")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, null, Description = "Internal server error")]
    [OpenApiOperation("CreateGameSession")]
    [Description("Creates a new game session")]
    public async Task<ActionResult<GameSessionDto>> CreateGameSessionAsync()
    {
        var gameSession = await _gameService.CreateGameSessionAsync();
        return Created($"api/game/{gameSession.Id}", gameSession);
    }

    [HttpGet("{id}/players")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<PlayerDto>), Description = "Returns all players in a game session")]
    [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "Game session not found")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, null, Description = "Internal server error")]
    [OpenApiOperation("GetPlayersByGameSessionId")]
    [Description("Returns all players in a game session")]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayersByGameSessionIdAsync(Guid id)
    {
        try
        {
            var players = await _gameService.GetPlayersByGameSessionIdAsync(id);
            return Ok(players);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id}/status")]
    [SwaggerResponse(HttpStatusCode.OK, null, Description = "Returns game status")]
    [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "Game session not found")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, null, Description = "Internal server error")]
    [OpenApiOperation("GetStatusByGameSessionId")]
    [Description("Returns status of the game")]

    public async Task<IActionResult> GetGameStatusAsync([FromRouteAttribute] string id)
    {
        throw new NotImplementedException();
    }

}
