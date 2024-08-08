using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAuthorizationAppService
{
    Task<Result<bool>> CanEditPlaylist(PlaylistId playlistId, PersonId? personId = null);
    
    Task<Result<bool>> IsManagementOrAdministrator(PersonId? personId = null);
    
    Task<Result<bool>> IsAdministrator(PersonId? personId = null);
}
