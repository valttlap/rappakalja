using Microsoft.EntityFrameworkCore;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Repositories;

public class GameRepository
{
    private readonly SanasoppaContext _context;

    public GameRepository(SanasoppaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a game session asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<GameSession> CreateGameSessionAsync()
    {
        var gameSession = new GameSession();
        var gameSessionEntity = await _context.GameSessions.AddAsync(gameSession);
        return gameSessionEntity.Entity;
    }

    /// <summary>
    /// Gets all game sessions asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<IEnumerable<GameSession>> GetGameSessionsAsync()
    {
        return await _context.GameSessions.ToListAsync();
    }

    /// <summary>
    /// Gets a game session by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the game session.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<GameSession?> GetGameSessionByIdAsync(Guid id)
    {
        return await _context.GameSessions.Include(g => g.Players).Include(g => g.Rounds).SingleOrDefaultAsync(g => g.Id == id);
    }

    /// <summary>
    /// Adds a player to a game session asynchronously.
    /// </summary>
    /// <param name="gameSessionId">The ID of the game session.</param>
    /// <param name="player">The player to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="AlreadyInGameException">Thrown when the player is already in the game session.</exception>
    /// <exception cref="NotFoundException">Thrown when the game session is not found.</exception>
    public async Task AddPlayerToGameSessionAsync(Guid gameSessionId, Player player)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
        if (gameSession.Players.Any(p => p.Id == player.Id))
        {
            throw new AlreadyInGameException($"Player with id {player.Id} is already in game session with id {gameSessionId}");
        }
        gameSession.OwnerId ??= player.Id;
        gameSession.Players.Add(player);
        Update(gameSession);
    }

    /// <summary>
    /// Removes a player from a game session asynchronously.
    /// </summary>
    /// <param name="gameSessionId">The ID of the game session.</param>
    /// <param name="playerId">The ID of the player to remove.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the game session or the player is not found.</exception>
    public async Task RemovePlayerFromGameSessionAsync(Guid gameSessionId, Guid playerId)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
        var player = gameSession.Players.SingleOrDefault(p => p.Id == playerId) ?? throw new NotFoundException($"Player with id {playerId} not found in game session with id {gameSessionId}");
        if (gameSession.OwnerId == player.Id)
        {
            gameSession.OwnerId = null;
        }
        gameSession.Players.Remove(player);
        Update(gameSession);
    }

    /// <summary>
    /// Starts a game asynchronously.
    /// </summary>
    /// <param name="gameSessionId">The ID of the game session.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the game session is not found.</exception>
    /// <exception cref="NotEnoughPlayersException">Thrown when there are not enough players in the game session.</exception>
    /// <exception cref="AlreadyStartedException">Thrown when the game session has already been started.</exception>
    public async Task StartGameAsync(Guid gameSessionId)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
        if (gameSession.StartTime != null)
        {
            throw new AlreadyStartedException($"Game session with id {gameSessionId} has already been started");
        }
        if (gameSession.Players.Count < 3)
        {
            throw new NotEnoughPlayersException($"Not enough players in game session with id {gameSessionId}");
        }
        gameSession.StartTime = DateTime.Now;
        Update(gameSession);
    }

    public void Update(GameSession gameSession)
    {
        _context.Entry(gameSession).State = EntityState.Modified;
    }
}
