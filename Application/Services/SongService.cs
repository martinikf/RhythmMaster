using Application.DTOs;
using Application.DTOs.Songs;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;

namespace Application.Services;

public class SongService(
    ISongRepository songRepository, 
    IAppUserRepository appUserRepository,
    IStorageService storageService
    ) : ISongService
{
    private readonly ISongRepository _songRepository = songRepository;
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    private readonly IStorageService _storageService = storageService;
    
    public async Task<Result<Song>> GetSongById(SongId songId)
    {
        var song = await _songRepository.GetByIdAsync(songId);
        
        if (song == null)
            return new NotFoundError<Song>($"Song with id: {songId} not found");
        
        return new SuccessResult<Song>(song);
    }

    public async Task<Result<PagedEnumerable<Song>>> GetSongs(GetSongsDto data)
    {
        var (songs, count) = await _songRepository.GetSongs(data);

        var pagedList = new PagedEnumerable<Song>(songs, data.PageSize, data.Page, count);
        
        return new SuccessResult<PagedEnumerable<Song>>(pagedList);
    }

    public async Task<Result> RemoveSong(SongId songId)
    {
        var song = await _songRepository.GetByIdAsync(songId);
        
        if (song == null)
            return new NotFoundError($"Song with id: {songId} not found");

        await _songRepository.Remove(song);
        
        var res = _storageService.DeleteFile(song);

        return res;
    }
    
    public async Task<Result> CreateSong(CreateSongDto data)
    {
        var song = new Song()
        {
            Name = data.Name,
            Duration = data.Duration,
            Artist = await _appUserRepository.GetByIdAsync(data.ArtistId),
            Bpm = data.Bpm,
            Power = data.Power,
            Price = data.Price ?? 0,
            SocialLink = data.SocialLink
        };

        await _songRepository.Add(song);
        
        //File upload
        await _storageService.UploadFileAsync(data.File, song);
        
        //Add SongDances
        await _songRepository.AddRangeOfDancesToSong(song, data.DancesIds);

        return new SuccessResult();
    }

    public async Task<Result> UpdateSong(UpdateSongDto data)
    {
        var song = await _songRepository.GetByIdAsync(data.Id);
        
        if (song == null)
            return new NotFoundError($"Song with id: {data.Id} not found");
        
        if(!string.IsNullOrEmpty(data.Name)) song.Name = data.Name;
        if(data.Duration != null) song.Duration = data.Duration;
        if(song.Artist != null) song.Artist = await _appUserRepository.GetByIdAsync(data.ArtistId);
        if(song.Bpm != null) song.Bpm = data.Bpm;
        if(song.Power != null) song.Power = data.Power;
        if(song.SocialLink != null) song.SocialLink = data.SocialLink;
        song.Price = data.Price ?? 0;

        await _songRepository.Update(song);

        await _songRepository.RemoveEveryFromSongAsync(song.Id);
        await _songRepository.AddRangeOfDancesToSong(song, data.DancesIds);

        return new SuccessResult();
    }
}