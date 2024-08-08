using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace API.OptionsSetup;

public class JwtOptionSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration = configuration;
    
    public void Configure(JwtOptions options)
    {
        _configuration.GetSection("Jwt").Bind(options);
    }
}