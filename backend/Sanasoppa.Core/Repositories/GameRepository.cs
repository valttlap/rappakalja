using System.Security.Cryptography;
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
        while (true)
        {
            var gameSession = new GameSession
            {
                JoinCode = GenerateRandomString(4)
            };
            if (!await JoinCodeExistsAsync(gameSession.JoinCode))
            {
                var gameSessionEntity = await _context.GameSessions.AddAsync(gameSession);
                return gameSessionEntity.Entity;
            }

        }
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

    public async Task<GameSession?> GetGameSessionByJoinCodeAsync(string joinCode)
    {
        return await _context.GameSessions.SingleOrDefaultAsync(g => g.JoinCode == joinCode);
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
        gameSession.StartTime = DateTime.UtcNow;
        Update(gameSession);
    }

    public async Task<Player?> GetOwnerAsync(Guid gameSessionId)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
        return gameSession.Players.SingleOrDefault(p => p.Id == gameSession.OwnerId);
    }

    public async Task SetOwnerAsync(Guid gameSessionId, Guid playerId)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
        var player = gameSession.Players.SingleOrDefault(p => p.Id == playerId) ?? throw new NotFoundException($"Player with id {playerId} not found in game session with id {gameSessionId}");
        gameSession.OwnerId = player.Id;
        Update(gameSession);
    }

    public void Update(GameSession gameSession)
    {
        _context.Entry(gameSession).State = EntityState.Modified;
    }

    private Task<bool> JoinCodeExistsAsync(string joinCode)
    {
        return _context.GameSessions.AnyAsync(g => g.JoinCode == joinCode);
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "aehijklmnoprstuvöä";
        using var rng = RandomNumberGenerator.Create();
        var stringChars = new char[length];
        var byteBuffer = new byte[1];

        for (int i = 0; i < stringChars.Length; i++)
        {
            int num;
            do
            {
                rng.GetBytes(byteBuffer);
                num = byteBuffer[0];
            } while (!IsValidCharValue(num, chars.Length));

            stringChars[i] = chars[num % chars.Length];
        }

        return new string(stringChars);
    }

    private static bool IsValidCharValue(int value, int max)
    {
        // MaxValue depends on the length of the character set used
        // Avoid numbers that are not evenly divisible by the character set length
        // to prevent character frequency bias
        int maxValue = 256 / max * max;
        return value < maxValue;
    }

}
