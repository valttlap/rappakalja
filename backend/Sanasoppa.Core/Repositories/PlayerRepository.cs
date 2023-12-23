using Microsoft.EntityFrameworkCore;
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
}
