using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MilkTea.Application.Ports.Authentication.JWTPort;
using System.Text;

namespace MilkTea.Infrastructure.Authentication.JWT
{
    public static class JWTMicrosoftRegistration
    {
        public static IServiceCollection AddAuthenticationJWTMicrosoft(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.Configure<JWTOptionDto>(configuration.GetSection("JwtSettings"));

            var jwtOptions = configuration.GetSection("JwtSettings").Get<JWTOptionDto>()
                                    ?? throw new InvalidOperationException("JwtSettings is missing or invalid.");

            services.AddScoped<IJWTServicePort, JWTMicrosoft>();

            services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = environment.IsProduction();
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });

            return services;
        }
    }
}
