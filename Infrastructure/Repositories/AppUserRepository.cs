using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class AppUserRepository : Repository<AppUser, PersonId>, IAppUserRepository
{
    public AppUserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<AppUser?> GetAppUserByIdAsync(PersonId id)
    {
        return await DbContext.DJs.FindAsync(id);
    }

    public async Task<AppUser?> GetAppUserByEmailAsync(string email)
    { 
        return await DbContext.DJs.FirstOrDefaultAsync(p => p.Email.Equals(email));
    }

    public async Task<bool> IsSongPurchasedAsync(PersonId userId, SongId songId)
    {
        return await DbContext.UserSongs.AnyAsync(us => us.AppUser.Id == userId && us.Song.Id == songId);
    }

    public Task PurchaseSongAsync(UserSong userSong)
    {
        DbContext.UserSongs.Add(userSong);
        return DbContext.SaveChangesAsync();
    }
}