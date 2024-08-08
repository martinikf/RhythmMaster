using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;

namespace Application.Services;

public class DanceService(
    IDanceRepository danceRepository
    ) : IDanceService
{
    private readonly IDanceRepository _danceRepository = danceRepository;

    public async Task<Result> CreateDanceAsync(string danceName)
    {
        if(string.IsNullOrWhiteSpace(danceName))
            return new ErrorResult("Dance name cannot be empty");
        
        var dance = await _danceRepository.GetDanceByNameAsync(danceName);
        
        if (dance != null)
            return new ErrorResult($"Dance with name: {danceName} already exists");

        var newDance = new Dance(danceName);
        
        await _danceRepository.Add(newDance);

        return new SuccessResult();
    }

    public async Task<Result> RemoveDanceAsync(DanceId id)
    {
        var dance = await _danceRepository.GetByIdAsync(id);

        if (dance == null)
            return new NotFoundError($"Dance with id: {id} not found");
        
        await _danceRepository.Remove(dance);
        
        return new SuccessResult();
    }

    public async Task<Result<Dance>> GetDanceByIdAsync(DanceId id)
    {
        var dance = await _danceRepository.GetByIdAsync(id);

        if (dance == null)
            return new NotFoundError<Dance>($"Dance with id: {id} not found");
        
        return new SuccessResult<Dance>(dance);
    }

    public async Task<Result<IEnumerable<Dance>>> GetAllDancesAsync()
    {
        var dances = await _danceRepository.GetAllDancesAsync();

        return new SuccessResult<IEnumerable<Dance>>(dances);
    }
}