using Application.DTOs;
using Application.DTOs.Persons;
using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ICreditService
{
    public Task<Result> UpdateCredit(UpdateCreditDto data);
    
    public Task<Result<int>> GetCreditBalance(PersonId userId);
}