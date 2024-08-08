using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IDanceRepository : IRepository<Dance, DanceId>
{
    public Task<Dance?> GetDanceByNameAsync(string name);
    
    public Task<IEnumerable<Dance>> GetAllDancesAsync();
}