using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IDanceService
{
    public Task<Result> CreateDanceAsync(string danceName);
    
    public Task<Result> RemoveDanceAsync(DanceId id);
    
    public Task<Result<Dance>> GetDanceByIdAsync(DanceId id);
    
    public Task<Result<IEnumerable<Dance>>> GetAllDancesAsync();
}