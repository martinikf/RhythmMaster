using Application.DTOs.Playlists;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class PlaylistService(
    IPlaylistRepository playlistRepository, 
    ISongRepository songRepository,
    ILoggedAppUserInfo loggedAppUserInfo
    ) : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository = playlistRepository;
    private readonly ISongRepository _songRepository = songRepository;
    private readonly ILoggedAppUserInfo _loggedAppUserInfo = loggedAppUserInfo;
    
    public async Task<Result> CreatePlaylistAsync(CreatePlaylistDto data)
    {
        var user = await _loggedAppUserInfo.GetLoggedInAppUserAsync();
        if(user is null) return new NotFoundError("Couldn't get logged in user.");
        
        var playlist = new Playlist(data.Name, data.Visibility ?? PlaylistVisibility.Personal, user, data.Description);
        await _playlistRepository.Add(playlist);
        
        return new SuccessResult();
    }

    public async Task<Result> RemovePlaylistAsync(PlaylistId playlistId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);

        if (playlist == null)
            return new NotFoundError($"Playlist with id: {playlistId} not found");

        await _playlistRepository.Remove(playlist);

        return new SuccessResult();
    }

    public async Task<Result> AddSongToPlaylistAsync(PlaylistSongDto data)
    {
        var existingSong = await _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId);
        if (existingSong != null)
            return new ErrorResult("Song already exists in playlist");
        
        var playlist = await _playlistRepository.GetByIdAsync(data.PlaylistId);
        if (playlist == null)
            return new NotFoundError($"Playlist with id: {data.PlaylistId} not found");
        
        var song = await _songRepository.GetByIdAsync(data.SongId);
        if (song == null)
            return new NotFoundError($"Song with id: {data.SongId} not found");
        
        var playlistSong = new PlaylistSong(playlist, song, playlist.Songs.Count);
        
        await _playlistRepository.AddSongToPlaylistAsync(playlistSong);
        
        return new SuccessResult();
    }

    public async Task<Result> RemoveSongFromPlaylistAsync(PlaylistSongDto data)
    {
        var playlistSong = await _playlistRepository.GetPlaylistSongAsync(data.PlaylistId, data.SongId);
        if (playlistSong == null)
            return new NotFoundError("Song not found in playlist");
        
        await _playlistRepository.RemoveSongFromPlaylist(playlistSong);
        
        return new SuccessResult();
    }

    public async Task<Result> ChangePlaylistVisibilityAsync(UpdatePlaylistVisibilityDto data)
    {
        var playlist = await _playlistRepository.GetByIdAsync(data.PlaylistId);
        
        if (playlist == null)
            return new NotFoundError($"Playlist with id: {data.PlaylistId} not found");
        
        playlist.Visibility = data.Visibility;
        
        await _playlistRepository.Update(playlist);
        
        return new SuccessResult();
    }
}