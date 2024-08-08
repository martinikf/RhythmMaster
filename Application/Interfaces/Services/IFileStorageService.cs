using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IStorageService
{
    Task<Result> UploadFileAsync(IFormFile file, Song song);
    
    Result DeleteFile(Song song);
}