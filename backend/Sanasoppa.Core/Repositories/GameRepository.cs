using Microsoft.EntityFrameworkCore;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.Model;

namespace Sanasoppa.Core.Repositories;

public class GameRepository
{
    private readonly SanasoppaContext _context;

    public GameRepository(SanasoppaContext context)
    {
        _context = context;
    }

    public async Task<GameSession> CreateGameSessionAsync()
    {
        var gameSession = new GameSession();
        var gameSessionEntity = await _context.GameSessions.AddAsync(gameSession);
        return gameSessionEntity.Entity;
    }

    public async Task<IEnumerable<GameSession>> GetGameSessionsAsync()
    {
        return await _context.GameSessions.ToListAsync();
    }

    public async Task<GameSession?> GetGameSessionByIdAsync(Guid id)
    {
        return await _context.GameSessions.Include(g => g.Players).Include(g => g.Rounds).SingleOrDefaultAsync(g => g.Id == id);
    }

    public async Task<bool> AddPlayerToGameSessionAsync(Guid gameSessionId, Player player)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId);
        if (gameSession == null)
        {
            return false;
        }
        if (gameSession.Players.Any(p => p.Id == player.Id))
        {
            return false;
        }
        gameSession.OwnerId ??= player.Id;
        gameSession.Players.Add(player);
        Update(gameSession);
        return true;
    }

    public async Task<bool> RemovePlayerFromGameSessionAsync(Guid gameSessionId, Guid playerId)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId);
        if (gameSession == null)
        {
            return false;
        }
        var player = gameSession.Players.SingleOrDefault(p => p.Id == playerId);
        if (player == null)
        {
            return false;
        }
        if (gameSession.OwnerId == player.Id)
        {
            gameSession.OwnerId = null;
        }
        gameSession.Players.Remove(player);
        Update(gameSession);
        return true;
    }

    public async Task StartGameAsync(Guid gameSessionId)
    {
        var gameSession = await _context.GameSessions.Include(g => g.Players).SingleOrDefaultAsync(g => g.Id == gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
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
