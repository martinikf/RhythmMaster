using Application.DTOs.Persons;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using FakeItEasy;
using FluentAssertions;

namespace Application.Tests.ServicesTests;

public class CreditServiceTests
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly ICreditService _creditService;
    
    public CreditServiceTests()
    {
        _appUserRepository = A.Fake<IAppUserRepository>();
        _creditService = new CreditService(_appUserRepository);
    }
    
    // UpdateCredit
    [Fact]
    public async Task UpdateCredit_ShouldReturnSuccess_WhenAppUserExists()
    {
        //Arrange
        var data = new UpdateCreditDto(new PersonId(1), 100);
        var appUser = new AppUser
        {
            CreditBalance = 0
        };
        A.CallTo(() => _appUserRepository.GetAppUserByIdAsync(new PersonId(1)))
            .Returns(Task.FromResult<AppUser?>(appUser));
        
        //Act
        var result = await _creditService.UpdateCredit(data);
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _appUserRepository.Update(A<AppUser>.That
                .Matches(u => u.CreditBalance == 100)))
            .MustHaveHappenedOnceExactly();
        Assert.Equal(100, appUser.CreditBalance);
    }
    
    [Fact]
    public async Task UpdateCredit_ShouldReturnNotFoundError_WhenAppUserDoesNotExist()
    {
        //Arrange
        var data = new UpdateCreditDto(new PersonId(1), 100);
        A.CallTo(() => _appUserRepository.GetAppUserByIdAsync(new PersonId(1)))
            .Returns(Task.FromResult<AppUser?>(null));
        
        //Act
        var result = await _creditService.UpdateCredit(data);
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
    
    // GetCreditBalance
    [Fact]
    public async Task GetCreditBalance_ShouldReturnSuccessWithCreditBalance_WhenAppUserExists()
    {
        //Arrange
        var appUser = new AppUser
        {
            CreditBalance = 100
        };
        A.CallTo(() => _appUserRepository.GetAppUserByIdAsync(new PersonId(1)))
            .Returns(Task.FromResult<AppUser?>(appUser));
        
        //Act
        var result = await _creditService.GetCreditBalance(new PersonId(1));
        
        //Assert
        Assert.IsType<SuccessResult<int>>(result);
        Assert.Equal(100, result.Data);
    }
    
    [Fact]
    public async Task GetCreditBalance_ShouldReturnNotFoundError_WhenAppUserDoesNotExist()
    {
        //Arrange
        A.CallTo(() => _appUserRepository.GetAppUserByIdAsync(new PersonId(1)))
            .Returns(Task.FromResult<AppUser?>(null));
        
        //Act
        var result = await _creditService.GetCreditBalance(new PersonId(1));
        
        //Assert
        Assert.IsType<NotFoundError<int>>(result);
    }
}