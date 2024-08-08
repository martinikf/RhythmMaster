using System.Security.Claims;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authorization;

public class AuthorizationAppService(ILoggedAppUserInfo loggedAppUser, ApplicationDbContext context)
    : IAuthorizationAppService
{
    private readonly ILoggedAppUserInfo _loggedAppUser = loggedAppUser;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<bool>> CanEditPlaylist(PlaylistId playlistId, PersonId? personId = null)
    {
        var role = await GetUserRole(personId);
        if(role is Role.Administrator or Role.Management) return new SuccessResult<bool>(true);

        if (personId is null)
            personId = _loggedAppUser.GetLoggedInPersonIdAsync();
        
        var playlist = await _context.Playlists
            .Include(x=>x.Owner)
            .FirstOrDefaultAsync(x=>x.Id.Equals(playlistId));

        if (playlist != null && playlist.Owner.Id.Equals(personId))
        {
            return new SuccessResult<bool>(true);
        }
        
        return new SuccessResult<bool>(false);
    }

    public async Task<Result<bool>> IsManagementOrAdministrator(PersonId? personId = null)
    {
        var role = await GetUserRole(personId);
        
        return role switch
        {
            Role.Administrator 
                or Role.Management => new SuccessResult<bool>(true),
            null => new ErrorResult<bool>("User not found"),
            _ => new SuccessResult<bool>(false)
        };
    }

    public async Task<Result<bool>> IsAdministrator(PersonId? personId = null)
    {
        var role = await GetUserRole(personId);
        
        return role switch
        {
            Role.Administrator => new SuccessResult<bool>(true),
            null => new ErrorResult<bool>("User not found"),
            _ => new SuccessResult<bool>(false)
        };
    }

    //Returns a role of a user or null, if user is not found.
    //If personId is provided, it will be used to get the user role. Else it will retrieve the id from the HttpContext.
    private async Task<Role?> GetUserRole(PersonId? personId)
    {
        if (personId is not null)
        {
            personId = _loggedAppUser.GetLoggedInPersonIdAsync();
        }
        
        return (await _context.DJs.FindAsync(personId))?.Role;
    }
}