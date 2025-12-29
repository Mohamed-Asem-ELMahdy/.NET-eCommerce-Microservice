using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eCommerce.SharedLibrary.DI;

public static class JWTAuthenticationScheme
{
    // this -> to be an extention method ----> AddJWTAuthenticationScheme(configuration)
    // configuration stores the app setting use like 
    // issuer, audience......
    
    public static IServiceCollection AddJWTAuthenticationScheme(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // extract setting from config.
            var key = configuration["JWT:Key"];
            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];
            
            // check the key.
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT Key is not configured");
            
            // here we check the token.
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

        return services;
    }
}
