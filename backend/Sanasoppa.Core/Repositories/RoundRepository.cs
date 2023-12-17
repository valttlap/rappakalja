using Microsoft.EntityFrameworkCore;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Repositories;

public class RoundRepository
{
    private readonly SanasoppaContext _context;

    public RoundRepository(SanasoppaContext context)
    {
        _context = context;
    }

    public async Task<Round> CreateAsync(Round round)
    {
        await _context.Rounds.AddAsync(round);
        await _context.SaveChangesAsync();
        return round;
    }

    public async Task<Round?> GetByIdAsync(Guid id)
    {
        return await _context.Rounds.FindAsync(id);
    }

    public async Task<IEnumerable<Round>> GetRoundsByGameSessionIdAsync(Guid gameSessionId)
    {
        return await _context.Rounds.Where(r => r.GameSessionId == gameSessionId).OrderBy(r => r.RoundNumber).ToListAsync();
    }

    public async Task<Round?> GetCurrentRoundByGameSessionIdAsync(Guid gameSessionId)
    {
        return await _context.Rounds.Where(r => r.GameSessionId == gameSessionId).OrderBy(r => r.RoundNumber).LastOrDefaultAsync();
    }

    public async Task AddWordToRoundAsync(Guid roundId, string word)
    {
        var round = await _context.Rounds.FindAsync(roundId) ?? throw new NotFoundException($"Round with id {roundId} not found");
        round.Word = word;
        Update(round);
    }

    public void Update(Round round)
    {
        _context.Entry(round).State = EntityState.Modified;
    }
}
