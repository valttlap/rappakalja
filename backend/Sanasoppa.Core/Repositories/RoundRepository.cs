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

    public async Task<Round> ResetRoundAsync(Guid roundId)
    {
        var round = await _context.Rounds.Include(r => r.Submissions).Where(r => r.Id == roundId).FirstOrDefaultAsync() ?? throw new NotFoundException($"Round with id {roundId} not found");
        round.Word = null;
        round.Submissions.Clear();
        Update(round);
        return round;
    }

    public async Task<Round> StartNewRound(Guid gameId)
    {
        var currentRound = await GetCurrentRoundByGameSessionIdAsync(gameId) ?? throw new NotFoundException($"Game session with id {gameId} not found");
        var players = await _context.Players.Where(p => p.GameSessionId == gameId).OrderBy(p => p.Id).ToListAsync();
        var leaderIndex = players.FindIndex(p => p.Id == currentRound.LeaderId);
        var newLeaderIndex = leaderIndex + 1 >= players.Count ? 0 : leaderIndex + 1;
        var newLeader = players[newLeaderIndex];
        var newRound = new Round
        {
            GameSessionId = gameId,
            RoundNumber = currentRound.RoundNumber + 1,
            LeaderId = newLeader.Id
        };
        await CreateAsync(newRound);
        return newRound;
    }

    public void Update(Round round)
    {
        _context.Entry(round).State = EntityState.Modified;
    }
}
