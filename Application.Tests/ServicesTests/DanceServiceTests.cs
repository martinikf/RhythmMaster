using Application.Interfaces.IRepositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using FakeItEasy;

namespace Application.Tests.ServicesTests;

public class DanceServiceTests
{
    
    private readonly IDanceRepository _danceRepository;
    private readonly IDanceService _danceService;
    
    public DanceServiceTests()
    {
        _danceRepository = A.Fake<IDanceRepository>();
        _danceService = new DanceService(_danceRepository);
    }

    // CreateDanceAsync
    [Fact]
    public async Task CreateDanceAsync_ShouldReturnSuccess_WhenDanceDoesNotExist()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetDanceByNameAsync("Salsa"))
            .Returns(Task.FromResult<Dance?>(null));
        
        //Act
        var result = await _danceService.CreateDanceAsync("Salsa");
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _danceRepository.Add(A<Dance>.That
            .Matches(d => d.Name == "Salsa")))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateDanceAsync_ShouldReturnError_WhenDanceAlreadyExists()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetDanceByNameAsync("Salsa"))
            .Returns(Task.FromResult<Dance?>(new Dance("Salsa")));
        
        //Act
        var result = await _danceService.CreateDanceAsync("Salsa");
        
        //Assert
        Assert.IsType<ErrorResult>(result);
    }

    [Fact]
    public async Task CreateDanceAsync_ShouldReturnError_WhenDanceNameIsEmpty()
    {
        //Arrange
        
        //Act
        var result = await _danceService.CreateDanceAsync("  ");
        
        //Assert
        Assert.IsType<ErrorResult>(result);
    }
    
    // RemoveDanceAsync
    [Fact]
    public async Task RemoveDanceAsync_ShouldReturnSuccess_WhenDanceExists()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetByIdAsync(A<DanceId>.Ignored))
            .Returns(Task.FromResult<Dance?>(new Dance("Salsa")));
        
        //Act
        var result = await _danceService.RemoveDanceAsync(new DanceId());
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _danceRepository.Remove(A<Dance>.That
                .Matches(d => d.Name == "Salsa")))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task RemoveDanceAsync_ShouldReturnNotFoundError_WhenDanceDoesntExist()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetByIdAsync(A<DanceId>.Ignored))
            .Returns(Task.FromResult<Dance?>(null));
        
        //Act
        var result = await _danceService.RemoveDanceAsync(new DanceId());
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
    
    // GetDanceByIdAsync
    [Fact]
    public async Task GetDanceByIdAsync_ShouldReturnSuccessDance_WhenDanceExists()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetByIdAsync(A<DanceId>.Ignored))
            .Returns(Task.FromResult<Dance?>(new Dance("Salsa")));
        
        //Act
        var result = await _danceService.GetDanceByIdAsync(new DanceId());
        
        //Assert
        Assert.IsType<SuccessResult<Dance>>(result);
        Assert.Equal("Salsa", result.Data.Name);
    }
    
    [Fact]
    public async Task GetDanceByIdAsync_ShouldReturnNotFoundError_WhenDanceDoesntExist()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetByIdAsync(A<DanceId>.Ignored))
            .Returns(Task.FromResult<Dance?>(null));
        
        //Act
        var result = await _danceService.GetDanceByIdAsync(new DanceId());
        
        //Assert
        Assert.IsType<NotFoundError<Dance>>(result);
    }
    
    // GetAllDancesAsync
    [Fact]
    public async Task GetAllDancesAsync_ShouldReturnSuccessDances_WhenDancesExist()
    {
        //Arrange
        var dances = new List<Dance>
        {
            new Dance("Salsa"),
            new Dance("Bachata")
        };
        
        A.CallTo(() => _danceRepository.GetAllDancesAsync())
            .Returns(Task.FromResult<IEnumerable<Dance>>(dances));
        
        //Act
        var result = await _danceService.GetAllDancesAsync();
        
        //Assert
        Assert.IsType<SuccessResult<IEnumerable<Dance>>>(result);
        Assert.Equal(2, result.Data.Count());
    }
    
    [Fact]
    public async Task GetAllDancesAsync_ShouldReturnSuccessDances_WhenNoDancesExist()
    {
        //Arrange
        A.CallTo(() => _danceRepository.GetAllDancesAsync())
            .Returns(Task.FromResult<IEnumerable<Dance>>(new List<Dance>()));
        
        //Act
        var result = await _danceService.GetAllDancesAsync();
        
        //Assert
        Assert.IsType<SuccessResult<IEnumerable<Dance>>>(result);
        Assert.Empty(result.Data);
    }
}