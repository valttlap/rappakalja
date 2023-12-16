using Microsoft.EntityFrameworkCore;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.model;

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
        return await _context.GameSessions.FindAsync(id);
    }
}
