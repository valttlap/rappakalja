using Microsoft.EntityFrameworkCore;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Repositories;

public class PlayerRepository
{
    private readonly SanasoppaContext _context;

    public PlayerRepository(SanasoppaContext context)
    {
        _context = context;
    }

    public async Task<Player> CreateAsync(Player player)
    {
        var playerEntity = await _context.Players.AddAsync(player);
        return playerEntity.Entity;
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        return await _context.Players.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Player?> GetByPlayerIdAsync(string id)
    {
        return await _context.Players.SingleOrDefaultAsync(p => p.PlayerId == id);
    }

    public async Task UpdatePlayerConnectionIdAsync(string playerId, string connectionId)
    {
        var player = await GetByPlayerIdAsync(playerId) ?? throw new NotFoundException("Player not found");
        player.ConnectionId = connectionId;
        Update(player);
    }

    public async Task<Player?> GetByConnectionIdAsync(string connectionId)
    {
        return await _context.Players.SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var player = await _context.Players.SingleOrDefaultAsync(p => p.Id == id);
        if (player is not null)
        {
            _context.Players.Remove(player);
        }
    }

    public async Task<bool> PlayerExistsAsync(string playerId)
    {
        return await _context.Players.AnyAsync(p => p.PlayerId == playerId);
    }

    public async Task UpdatePlayerGameAsync(string playerId, Guid gameGuid)
    {
        var player = await GetByPlayerIdAsync(playerId) ?? throw new NotFoundException("Player not found");
        player.GameSessionId = gameGuid;
        Update(player);
    }

    public void Update(Player player)
    {
        _context.Entry(player).State = EntityState.Modified;
    }
}
