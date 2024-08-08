using Application.DTOs;
using Application.DTOs.Playlists;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IPlaylistService
{
    public Task<Result> CreatePlaylistAsync(CreatePlaylistDto data);
    
    public Task<Result> RemovePlaylistAsync(PlaylistId playlistId);
    
    public Task<Result> AddSongToPlaylistAsync(PlaylistSongDto data);
    
    //public Task<Result> AddSongsToPlaylist(PlaylistId playlistId, IEnumerable<SongId> songs);
    
    public Task<Result> RemoveSongFromPlaylistAsync(PlaylistSongDto data);
    
    public Task<Result> ChangePlaylistVisibilityAsync(UpdatePlaylistVisibilityDto data);
}