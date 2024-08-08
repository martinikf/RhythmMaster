using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class DanceRepository : Repository<Dance, DanceId>, IDanceRepository
{
    public DanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Dance?> GetDanceByNameAsync(string name)
    {
        return await DbContext.Dances.FirstOrDefaultAsync(dance => dance.Name.Equals(name));
    }

    public async Task<IEnumerable<Dance>> GetAllDancesAsync()
    {
        return await DbContext.Dances.ToListAsync();
    }
    
}