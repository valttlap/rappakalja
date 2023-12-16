using Sanasoppa.Model.Context;

namespace Sanasoppa.Core.Repositories;

public class UnitOfWork
{
    private readonly SanasoppaContext _context;

    public UnitOfWork(SanasoppaContext context)
    {
        _context = context;
    }

    public GameRepository GameRepository => new(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
