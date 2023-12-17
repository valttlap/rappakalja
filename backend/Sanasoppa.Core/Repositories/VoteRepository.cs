using Microsoft.EntityFrameworkCore;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Repositories;

public class VoteRepository
{
    private SanasoppaContext _context;

    public VoteRepository(SanasoppaContext context)
    {
        _context = context;
    }

    public async Task<Vote> CreateAsync(Vote vote)
    {
        var voteEntity = await _context.Votes.AddAsync(vote);
        return voteEntity.Entity;
    }

    public async Task<IEnumerable<Vote>> GetVotesByRoundIdAsync(Guid roundId)
    {
        return await _context.Votes.Where(v => v.RoundId == roundId).ToListAsync();
    }

    public async Task<IEnumerable<Vote>> GetVotesBySubmissionIdAsync(Guid submissionId)
    {
        return await _context.Votes.Where(v => v.SubmissionId == submissionId).ToListAsync();
    }

    public async Task<IEnumerable<Vote>> GetVotesByPlayerIdAsync(Guid playerId)
    {
        return await _context.Votes.Where(v => v.VoterId == playerId).ToListAsync();
    }
}
