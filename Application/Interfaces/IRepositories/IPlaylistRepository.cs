using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IPlaylistRepository : IRepository<Playlist, PlaylistId>
{
    
    Task AddSongToPlaylistAsync(PlaylistSong playlistSong);

    Task AddSongsToPlaylistAsync(Playlist playlist, IEnumerable<Song> songs);
    
    Task RemoveSongFromPlaylist(PlaylistSong playlistSong);
    
    Task<PlaylistSong?> GetPlaylistSongAsync(PlaylistId playlistId, SongId songId);
    
}