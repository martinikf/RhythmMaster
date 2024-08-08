using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

public class StorageService(IOptions<StorageOptions> options) : IStorageService
{
    private readonly StorageOptions _options = options.Value;
    
    public async Task<Result> UploadFileAsync(IFormFile file, Song song)
    {
        if (file.Length == 0)
            return new ErrorResult("File is empty.");

        var filePath = FilePath(song);

        try
        {
            var directoryName = Path.GetDirectoryName(filePath);
            if(directoryName == null) return new ErrorResult("Invalid file path.");
            
            Directory.CreateDirectory(directoryName); // Ensure the directory exists

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new SuccessResult();
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while uploading the file: {ex.Message}");
        }
    }

    public Result DeleteFile(Song song)
    {
        var filePath = FilePath(song);
        
        if (string.IsNullOrEmpty(filePath))
            return new ErrorResult("Invalid song or file path.");

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                return new ErrorResult("File does not exist.");
            }

            return new SuccessResult();
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while deleting the file: {ex.Message}");
        }
    }

    private string FilePath(Song song)
    {
        return Path.Combine(_options.PathToStorageRoot, song.Id.ToString());
    }
}