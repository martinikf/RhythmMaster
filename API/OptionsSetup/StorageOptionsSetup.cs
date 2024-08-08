using Microsoft.Extensions.Options;

namespace API.OptionsSetup;

public class StorageOptionsSetup(IConfiguration configuration) : IConfigureOptions<StorageOptionsSetup>
{
    private readonly IConfiguration _configuration = configuration;
    
    public void Configure(StorageOptionsSetup options)
    {
        _configuration.GetSection("Storage").Bind(options);
    }
}