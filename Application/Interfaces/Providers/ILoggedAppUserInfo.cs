using Domain.Entities;

namespace Application.Interfaces.Providers;

public interface ILoggedAppUserInfo
{
    Task<AppUser?> GetLoggedInAppUserAsync();

    public PersonId? GetLoggedInPersonIdAsync();
}