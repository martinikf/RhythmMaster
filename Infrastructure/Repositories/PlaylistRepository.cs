using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PlaylistRepository : Repository<Playlist, PlaylistId>, IPlaylistRepository
{
    public PlaylistRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddSongToPlaylistAsync(PlaylistSong playlistSong)
    {
        await DbContext.PlaylistSongs.AddAsync(playlistSong);
        await DbContext.SaveChangesAsync();
    }

    public async Task AddSongsToPlaylistAsync(Playlist playlist, IEnumerable<Song> songs)
    {
        var i = 0;
        await DbContext.PlaylistSongs.AddRangeAsync(songs.Select(song => new PlaylistSong(playlist, song, i++)));
        await DbContext.SaveChangesAsync();
    }

    public async Task RemoveSongFromPlaylist(PlaylistSong playlistSong)
    {
        DbContext.PlaylistSongs.Remove(playlistSong);
        await DbContext.SaveChangesAsync();
    }

    public async Task ChangePlaylistVisibilityAsync(Playlist playlist, PlaylistVisibility visibility)
    {
        playlist.Visibility = visibility;
        DbContext.Playlists.Update(playlist);
        await DbContext.SaveChangesAsync();
    }

    public async Task<PlaylistSong?> GetPlaylistSongAsync(PlaylistId playlistId, SongId songId)
    {
        return await DbContext.PlaylistSongs.FirstOrDefaultAsync(ps => ps.Playlist.Id == playlistId && ps.Song.Id == songId);
    }
}