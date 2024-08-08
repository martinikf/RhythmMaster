using Application.DTOs.Playlists;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("playlist")]
[ApiController]
public class PlaylistController(IAuthorizationAppService authorizationService, IPlaylistService playlistService) : ControllerBase
{
    private readonly IAuthorizationAppService _authorizationService = authorizationService;
    private readonly IPlaylistService _playlistService = playlistService;
    
    [Authorize]
    [HttpPost("createPlaylist")]
    public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto data)
    {
        var result = await _playlistService.CreatePlaylistAsync(data);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize]
    [HttpPost("removePlaylist")]
    public async Task<IActionResult> RemovePlaylist([FromBody] PlaylistId playlistId)
    {
        var allowedAction = await _authorizationService.CanEditPlaylist(playlistId);
        if (!allowedAction.Data)
            return Forbid();
        
        var result = await _playlistService.RemovePlaylistAsync(playlistId);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize]
    [HttpPost("addSongToPlaylist")]
    public async Task<IActionResult> AddSongToPlaylist([FromBody] PlaylistSongDto data)
    {
        var allowedAction = await _authorizationService.CanEditPlaylist(data.PlaylistId);
        if (!allowedAction.Data)
            return Forbid();
        
        var result = await _playlistService.AddSongToPlaylistAsync(data);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize]
    [HttpPost("removeSongFromPlaylist")]
    public async Task<IActionResult> RemoveSongFromPlaylist([FromBody] PlaylistSongDto data)
    {
        var allowedAction = await _authorizationService.CanEditPlaylist(data.PlaylistId);
        if (!allowedAction.Data)
            return Forbid();
        
        var result = await _playlistService.RemoveSongFromPlaylistAsync(data);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize]
    [HttpPost("changePlaylistVisibility")]
    public async Task<IActionResult> ChangePlaylistVisibility([FromBody] UpdatePlaylistVisibilityDto data)
    {
        var allowedAction = await _authorizationService.CanEditPlaylist(data.PlaylistId);
        if (!allowedAction.Data)
            return Forbid();
        
        var result = await _playlistService.ChangePlaylistVisibilityAsync(data);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
}