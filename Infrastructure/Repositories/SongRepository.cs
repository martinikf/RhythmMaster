using System.Linq.Expressions;
using Application.DTOs;
using Application.DTOs.Songs;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class SongRepository :  Repository<Song, SongId>, ISongRepository
{
    public SongRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<(IEnumerable<Song>, int)> GetSongs(GetSongsDto data)
    {
        var songsQ = DbContext.Songs.AsQueryable();
        
        //Search term, ignores case, todo: ignore diacritics
        var searchTerm = data.SearchTerm?.ToLower();
        if(!string.IsNullOrWhiteSpace(searchTerm))
            songsQ = songsQ.Where(x=>x.Name.ToLower().Contains(searchTerm));
        
        //Sorting
        if(data.SortAscending)
            songsQ = songsQ.OrderBy(GetSortColumnProperty(data.SortByFieldName));
        else
            songsQ = songsQ.OrderByDescending(GetSortColumnProperty(data.SortByFieldName));

        var count = await songsQ.CountAsync();
        
        //Pagination, indexed from 1
        songsQ = songsQ.Skip((data.Page - 1) * data.PageSize).Take(data.PageSize);
        
        return (await songsQ.ToListAsync(), count);
    }
    
    private static Expression<Func<Song, object>> GetSortColumnProperty(string sortByFieldName)
    {
        sortByFieldName = sortByFieldName.Trim().ToLower();
        //TODO: Add more fields, artist name etc.
        return sortByFieldName switch
        {
            "name" => song => song.Name,
            "price" => song => song.Price,
            _ => song => song.Name
        };
    }

    public async Task RemoveEveryFromSongAsync(SongId songId)
    {
        await DbContext.SongDances
            .Where(sd => sd.Song.Id == songId)
            .ForEachAsync(x=> DbContext.SongDances.Remove(x));
        
        await DbContext.SaveChangesAsync();
    }

    public async Task<int> AddRangeOfDancesToSong(Song song, IEnumerable<DanceId> songDances)
    {
        foreach (var danceId in songDances)
        {
            var dance = await DbContext.Dances.FindAsync(danceId);
            if (dance == null)
                continue;

            var sd = new SongDance(song, dance);
            DbContext.SongDances.Add(sd);
        }
        
        return await DbContext.SaveChangesAsync();
    }
}