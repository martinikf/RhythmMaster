using Domain.Entities;

namespace Application.Interfaces.Providers;

public interface IJwtProvider
{
    string Generate(AppUser appUser);
}