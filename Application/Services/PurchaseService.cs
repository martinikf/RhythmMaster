using Application.DTOs;
using Application.DTOs.Persons;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;

namespace Application.Services;

public class PurchaseService(
    IAppUserRepository appUserRepository, 
    ISongRepository songRepository,
    ICreditService creditService
    ) : IPurchaseService
{
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    private readonly ISongRepository _songRepository = songRepository;
    private readonly ICreditService _creditService = creditService;
    
    public async Task<Result> PurchaseSong(PurchaseSongDto data)
    {
        var user = await _appUserRepository.GetAppUserByIdAsync(data.UserId);
        if (user == null) return new NotFoundError("User not found");

        var song = await _songRepository.GetByIdAsync(data.SongId);
        if (song == null) return new NotFoundError("Song not found");
        
        if (user.CreditBalance < song.Price) 
            return new ErrorResult("Insufficient credits");
        
        if(await _appUserRepository.IsSongPurchasedAsync(data.UserId, data.SongId)) 
            return new ErrorResult("Song already purchased");

        var us = new UserSong(user, song);
        await _appUserRepository.PurchaseSongAsync(us);

        var result = await _creditService.UpdateCredit(new UpdateCreditDto(user.Id, -song.Price));

        return result switch
        {
            SuccessResult => new SuccessResult(),
            ErrorResult errorResult => new ErrorResult(errorResult.Message),
            _ => new ErrorResult("An unknown error occurred")
        };
    }
}