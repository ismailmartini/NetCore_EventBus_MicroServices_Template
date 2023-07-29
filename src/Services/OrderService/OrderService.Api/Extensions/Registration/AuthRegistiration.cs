using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OrderService.Api.Extensions
{
    public static class AuthRegistration
    {

        public static IServiceCollection ConfigureAuth(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthConfig:Secret"]));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    { 
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireSignedTokens=true,
                    ClockSkew=TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey
                    };
             });
                return services;
            
        }
    }
}
