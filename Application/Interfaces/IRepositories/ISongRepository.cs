using Application.DTOs;
using Application.DTOs.Songs;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ISongRepository : IRepository<Song, SongId>
{
    Task<(IEnumerable<Song>, int)> GetSongs(GetSongsDto data);
    
    Task RemoveEveryFromSongAsync(SongId songId);
    
    Task<int> AddRangeOfDancesToSong(Song song, IEnumerable<DanceId> songDances);
}