using System.Security.Claims;
using Application.Interfaces.Providers;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class LoggedAppUserInfo(IHttpContextAccessor httpContext, ApplicationDbContext dbContext) : ILoggedAppUserInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContext;
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public async Task<AppUser?> GetLoggedInAppUserAsync()
    {
        var id = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id is null) return null;
        
        var result = int.TryParse(id, out var intId);
        if (!result) return null;
        
        return await _dbContext.DJs.FirstOrDefaultAsync(u => u.Id == new PersonId(intId));
    }
    
    public PersonId? GetLoggedInPersonIdAsync()
    {
        var id = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id is null) return null;
        
        var result = int.TryParse(id, out var intId);
        if (!result) return null;
        
        return new PersonId(intId);
    }
}