using Application.DTOs;
using Application.DTOs.Persons;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces.Services;

public interface IPurchaseService
{
    public Task<Result> PurchaseSong(PurchaseSongDto data);
}