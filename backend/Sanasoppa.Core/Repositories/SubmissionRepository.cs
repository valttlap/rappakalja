using Microsoft.EntityFrameworkCore;
using Sanasoppa.Model.Context;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Repositories;

public class SubmissionRepository
{
    private readonly SanasoppaContext _context;

    public SubmissionRepository(SanasoppaContext context)
    {
        _context = context;
    }

    public async Task<Submission> CreateAsync(Submission submission)
    {
        var submissionEntity = await _context.Submissions.AddAsync(submission);
        return submissionEntity.Entity;
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByRoundIdAsync(Guid roundId)
    {
        return await _context.Submissions.Include(s => s.Player).Where(s => s.RoundId == roundId).ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetNotCorrectSubmissionsByRoundIdAsync(Guid roundId)
    {
        var roundLeader = await _context.Rounds.Where(r => r.Id == roundId).Select(r => r.LeaderId).SingleOrDefaultAsync();
        return await _context.Submissions.Where(s => s.RoundId == roundId && s.PlayerId != roundLeader).ToListAsync();
    }
}
