using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IAppUserRepository : IRepository<AppUser, PersonId>
{
    Task<AppUser?> GetAppUserByIdAsync(PersonId id);
    
    Task<AppUser?> GetAppUserByEmailAsync(string email);
    
    Task<bool> IsSongPurchasedAsync(PersonId userId, SongId songId);
    
    Task PurchaseSongAsync(UserSong userSong);
}