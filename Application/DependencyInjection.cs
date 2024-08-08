using System.Reflection;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IDanceService, DanceService>();
        services.AddTransient<ISongService, SongService>();
        services.AddTransient<IPlaylistService, PlaylistService>();
        services.AddTransient<ICreditService, CreditService>();
        services.AddTransient<IPurchaseService, PurchaseService>();
        
        return services;
    }
}