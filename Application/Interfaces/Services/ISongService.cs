using Application.DTOs;
using Application.DTOs.Songs;
using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ISongService
{
    public Task<Result<Song>> GetSongById(SongId songId);
    
    public Task<Result<PagedEnumerable<Song>>> GetSongs(GetSongsDto data);
    
    public Task<Result> CreateSong(CreateSongDto data);
    
    public Task<Result> RemoveSong(SongId songId);
    
    public Task<Result> UpdateSong(UpdateSongDto data);
}