using Application.DTOs.Playlists;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using FakeItEasy;

namespace Application.Tests.ServicesTests;

public class PlaylistServiceTests
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ISongRepository _songRepository;
    private readonly ILoggedAppUserInfo _loggedAppUserInfo;
    
    private readonly IPlaylistService _playlistService;
    
    
    public PlaylistServiceTests()
    {
        _playlistRepository = A.Fake<IPlaylistRepository>();
        _songRepository = A.Fake<ISongRepository>();
        _loggedAppUserInfo = A.Fake<ILoggedAppUserInfo>();
        _playlistService = new PlaylistService(_playlistRepository, _songRepository, _loggedAppUserInfo);
    }
    
    // CreatePlaylistAsync
    [Fact]
    public async Task CreatePlaylistAsync_ShouldReturnSuccess_WhenUserExists()
    {
        //Arrange
        var data = new CreatePlaylistDto("Playlist", "Description", PlaylistVisibility.Everyone);
        var user = new AppUser();
        A.CallTo(() => _loggedAppUserInfo.GetLoggedInAppUserAsync()).Returns(Task.FromResult<AppUser?>(user));
        
        //Act
        var result = await _playlistService.CreatePlaylistAsync(data);
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _playlistRepository.Add(A<Playlist>.That
                .Matches(p => p.Name == "Playlist" && p.Description == "Description" && p.Visibility == PlaylistVisibility.Everyone)))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task RemovePlaylistAsync_ShouldReturnSuccess_WhenPlaylistExists()
    {
        //Arrange
        var playlistId = new PlaylistId(1);
        var playlist = new Playlist(playlistId, "Playlist", PlaylistVisibility.Everyone, new AppUser(), null);
        A.CallTo(() => _playlistRepository.GetByIdAsync(playlistId))
            .Returns(Task.FromResult<Playlist?>(playlist));
        
        //Act
        var result = await _playlistService.RemovePlaylistAsync(playlistId);
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _playlistRepository.Remove(A<Playlist>.That
                .Matches(p => p.Id == playlistId)))
            .MustHaveHappenedOnceExactly();
    }
    
    // RemovePlaylistAsync
    [Fact]
    public async Task RemovePlaylistAsync_ShouldReturnNotFoundError_WhenPlaylistDoesNotExist()
    {
        //Arrange
        var playlistId = new PlaylistId(1);
        A.CallTo(() => _playlistRepository.GetByIdAsync(playlistId))
            .Returns(Task.FromResult<Playlist?>(null));
        
        //Act
        var result = await _playlistService.RemovePlaylistAsync(playlistId);
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
    
    // AddSongToPlaylistAsync
    [Fact]
    public async Task AddSongToPlaylistAsync_ShouldReturnSuccess_WhenSongAndPlaylistExist()
    {
        //Arrange
        var data = new PlaylistSongDto(new PlaylistId(1), new SongId(1));
        var playlist = new Playlist(new PlaylistId(1), "Playlist", PlaylistVisibility.Everyone, new AppUser(), null);
        var song = new Song(new SongId(1));
        A.CallTo(() => _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId))
            .Returns(Task.FromResult<PlaylistSong?>(null));
        A.CallTo(() => _playlistRepository.GetByIdAsync(data.PlaylistId))
            .Returns(Task.FromResult<Playlist?>(playlist));
        A.CallTo(() => _songRepository.GetByIdAsync(data.SongId))
            .Returns(Task.FromResult<Song?>(song));
        
        //Act
        var result = await _playlistService.AddSongToPlaylistAsync(data);
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _playlistRepository.AddSongToPlaylistAsync(A<PlaylistSong>.That
                .Matches(ps => ps.Playlist.Id == data.PlaylistId && ps.Song.Id == data.SongId)))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task AddSongToPlaylistAsync_ShouldReturnError_WhenSongAlreadyExistsInPlaylist()
    {
        //Arrange
        var data = new PlaylistSongDto(new PlaylistId(1), new SongId(1));
        var playlistSong = new PlaylistSong(new Playlist(new PlaylistId(1), "Playlist", PlaylistVisibility.Everyone, new AppUser(), null), new Song(new SongId(1)), 0);
        A.CallTo(() => _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId))
            .Returns(Task.FromResult<PlaylistSong?>(playlistSong));
        
        //Act
        var result = await _playlistService.AddSongToPlaylistAsync(data);
        
        //Assert
        Assert.IsType<ErrorResult>(result);
    }
    
    [Fact]
    public async Task AddSongToPlaylistAsync_ShouldReturnNotFoundError_WhenPlaylistDoesNotExist()
    {
        //Arrange
        var data = new PlaylistSongDto(new PlaylistId(1), new SongId(1));
        A.CallTo(() => _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId))
            .Returns(Task.FromResult<PlaylistSong?>(null));
        A.CallTo(() => _playlistRepository.GetByIdAsync(data.PlaylistId))
            .Returns(Task.FromResult<Playlist?>(null));
        
        //Act
        var result = await _playlistService.AddSongToPlaylistAsync(data);
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
    
    [Fact]
    public async Task AddSongToPlaylistAsync_ShouldReturnNotFoundError_WhenSongDoesNotExist()
    {
        //Arrange
        var data = new PlaylistSongDto(new PlaylistId(1), new SongId(1));
        A.CallTo(() => _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId))
            .Returns(Task.FromResult<PlaylistSong?>(null));
        A.CallTo(() => _playlistRepository.GetByIdAsync(data.PlaylistId))
            .Returns(Task.FromResult<Playlist?>(new Playlist(new PlaylistId(1), "Playlist", PlaylistVisibility.Everyone, new AppUser(), null)));
        A.CallTo(() => _songRepository.GetByIdAsync(data.SongId))
            .Returns(Task.FromResult<Song?>(null));
        
        //Act
        var result = await _playlistService.AddSongToPlaylistAsync(data);
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
    
    // RemoveSongFromPlaylistAsync
    [Fact]
    public async Task RemoveSongFromPlaylistAsync_ShouldReturnSuccess_WhenSongExistsInPlaylist()
    {
        //Arrange
        var data = new PlaylistSongDto(new PlaylistId(1), new SongId(1));
        var playlistSong = new PlaylistSong(new Playlist(new PlaylistId(1), "Playlist", PlaylistVisibility.Everyone, new AppUser(), null), new Song(new SongId(1)), 0);
        A.CallTo(() => _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId))
            .Returns(Task.FromResult<PlaylistSong?>(playlistSong));
        
        //Act
        var result = await _playlistService.RemoveSongFromPlaylistAsync(data);
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _playlistRepository.RemoveSongFromPlaylist(A<PlaylistSong>.That
                .Matches(ps => ps.Playlist.Id == data.PlaylistId && ps.Song.Id == data.SongId)))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task RemoveSongFromPlaylistAsync_ShouldReturnNotFoundError_WhenSongDoesNotExistInPlaylist()
    {
        //Arrange
        var data = new PlaylistSongDto(new PlaylistId(1), new SongId(1));
        A.CallTo(() => _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId))
            .Returns(Task.FromResult<PlaylistSong?>(null));
        
        //Act
        var result = await _playlistService.RemoveSongFromPlaylistAsync(data);
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
    
    // ChangePlaylistVisibilityAsync
    [Fact]
    public async Task ChangePlaylistVisibilityAsync_ShouldReturnSuccess_WhenPlaylistExists()
    {
        //Arrange
        var data = new UpdatePlaylistVisibilityDto(new PlaylistId(1), PlaylistVisibility.Personal);
        var playlist = new Playlist(new PlaylistId(1), "Playlist", PlaylistVisibility.Everyone, new AppUser(), null);
        A.CallTo(() => _playlistRepository.GetByIdAsync(data.PlaylistId))
            .Returns(Task.FromResult<Playlist?>(playlist));
        
        //Act
        var result = await _playlistService.ChangePlaylistVisibilityAsync(data);
        
        //Assert
        Assert.IsType<SuccessResult>(result);
        A.CallTo(() => _playlistRepository.Update(A<Playlist>.That
                .Matches(p => p.Id == data.PlaylistId && p.Visibility == PlaylistVisibility.Personal)))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task ChangePlaylistVisibilityAsync_ShouldReturnNotFoundError_WhenPlaylistDoesNotExist()
    {
        //Arrange
        var data = new UpdatePlaylistVisibilityDto(new PlaylistId(1), PlaylistVisibility.Personal);
        A.CallTo(() => _playlistRepository.GetByIdAsync(data.PlaylistId))
            .Returns(Task.FromResult<Playlist?>(null));
        
        //Act
        var result = await _playlistService.ChangePlaylistVisibilityAsync(data);
        
        //Assert
        Assert.IsType<NotFoundError>(result);
    }
}