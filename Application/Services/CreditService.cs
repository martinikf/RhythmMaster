using Application.DTOs;
using Application.DTOs.Persons;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;

namespace Application.Services;

public class CreditService(
    IAppUserRepository appUserRepository
    ) : ICreditService
{
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    
    public async Task<Result> UpdateCredit(UpdateCreditDto data)
    {
        var appUser = await _appUserRepository.GetAppUserByIdAsync(data.UserId);

        if (appUser is null)
        {
            return new NotFoundError("User not found.");
        }

        appUser.CreditBalance += data.CreditDifference;
        await _appUserRepository.Update(appUser);
        return new SuccessResult();
    }
    
    public async Task<Result<int>> GetCreditBalance(PersonId userId)
    {
        var appUser = await _appUserRepository.GetAppUserByIdAsync(userId);

        if (appUser is null)
        {
            return new NotFoundError<int>("User not found.");
        }
        
        return new SuccessResult<int>(appUser.CreditBalance);
            
    }
}