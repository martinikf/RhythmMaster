using Application.DTOs.Songs;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("song")]
[ApiController]
public class SongController(ISongService songService) : ControllerBase
{
    private readonly ISongService _songService = songService;
    
    [HttpGet("getSongById")]
    public async Task<IActionResult> GetSongById([FromQuery] SongId songId)
    {
        var result = await _songService.GetSongById(songId);

        return result switch
        {
            SuccessResult<Song> successResult => Ok(successResult.Data),
            NotFoundError<Song> notFoundResult => NotFound(notFoundResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [HttpGet("getSongs")]
    public async Task<IActionResult> GetSongs([FromBody] GetSongsDto data)
    {
        var result = await _songService.GetSongs(data);

        return result switch
        {
            SuccessResult<PagedEnumerable<Song>> successResult => Ok(successResult.Data),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize(Roles=$"{nameof(Role.InternalDJ)},{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("createSong")]
    public async Task<IActionResult> CreateSong([FromBody] CreateSongDto data)
    {
        var result = await _songService.CreateSong(data);

        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize(Roles=$"{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("removeSong")]
    public async Task<IActionResult> RemoveSong([FromBody] SongId songId)
    {
        var result = await _songService.RemoveSong(songId);

        return result switch
        {
            SuccessResult => Ok(),
            NotFoundError errorResult => NotFound(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize(Roles=$"{nameof(Role.InternalDJ)},{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("updateSong")]
    public async Task<IActionResult> UpdateSong([FromBody] UpdateSongDto data)
    {
        var result = await _songService.UpdateSong(data);

        return result switch
        {
            SuccessResult => Ok(),
            NotFoundError errorResult => NotFound(errorResult.Message),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
}