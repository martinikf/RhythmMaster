using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Common;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {  
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddTransient<IPasswordHashProvider, PasswordHashProvider>();
        
        services.AddTransient<IAuthorizationAppService, AuthorizationAppService>();
        services.AddTransient<IStorageService, StorageService>();
        
        services.AddTransient<ILoggedAppUserInfo, LoggedAppUserInfo>();
        
        //Repositories
        services.AddScoped<IDanceRepository, DanceRepository>();
        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IInterpretRepository, InterpretRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        
        
        return services;
    }
}
