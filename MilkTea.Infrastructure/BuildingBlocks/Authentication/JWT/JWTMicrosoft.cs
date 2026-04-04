using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MilkTea.Infrastructure.BuildingBlocks.Authentication.JWT;


public class JWTOption
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpireMinutes { get; set; }
    public int RefreshTokenExpireMinutes { get; set; }
}


public static class JWTMicrosoftRegistration
{
    public static IServiceCollection AddAuthenticationJWTMicrosoft(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<JWTOption>(configuration.GetSection("JwtSettings"));

        var jwtOptions = configuration.GetSection("JwtSettings").Get<JWTOption>()
                                ?? throw new InvalidOperationException("JwtSettings is missing or invalid.");

        services.AddScoped<IJWTServicePort, JWTMicrosoft>();

        #region Configure JWT Authentication
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
                options.Events = new JwtBearerEvents
                {
                    // Receive Token
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                        Console.WriteLine($"[JWT] Received token: {token?.Substring(0, Math.Min(50, token?.Length ?? 0))}...");
                        return Task.CompletedTask;
                    },
                    // Pass Authentication
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine($"[JWT] Token validated successfully for user: {context.Principal?.Identity?.Name}");
                        return Task.CompletedTask;
                    },
                    // Sended token but failed to authenticate
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    // Sended token but failed to authenticate, and the response is about to be sent back to client
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"[JWT] OnChallenge triggered: {context.Error} - {context.ErrorDescription}");
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            message = "Unauthorized",
                            statusCode = 401,
                            timestamp = DateTime.Now,
                            error = context.Error,
                            errorDescription = context.ErrorDescription
                        });
                        return context.Response.WriteAsync(result);
                    }
                };
            });
        #endregion Configure JWT Authentication

        return services;
    }
}

public class JWTMicrosoft(IOptionsSnapshot<JWTOption> jwtOptions) : IJWTServicePort
{
    private readonly IOptionsSnapshot<JWTOption> _vJwtOptions = jwtOptions;
    public (string AccessToken, DateTime ExpiresAt) CreateJwtAccessToken(int userId, Dictionary<string, object>? additionalClaims = null)
    {
        var vClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new("UserID", userId.ToString()),
            };
        var vKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_vJwtOptions.Value.Key));
        var vCreds = new SigningCredentials(vKey, SecurityAlgorithms.HmacSha256);
        var vExpires = DateTime.Now.AddMinutes(_vJwtOptions.Value.AccessTokenExpireMinutes);

        var vToken = new JwtSecurityToken(
            issuer: _vJwtOptions.Value.Issuer,
            audience: _vJwtOptions.Value.Audience,
            claims: vClaims,
            expires: vExpires,
            signingCredentials: vCreds);

        return (new JwtSecurityTokenHandler().WriteToken(vToken), vExpires);
    }

    public (string RefreshToken, DateTime ExpiresAt) CreateJwtRefreshToken()
    {
        var vRandomBytes = new byte[64];
        using var vRng = RandomNumberGenerator.Create();
        vRng.GetBytes(vRandomBytes);
        return (Convert.ToBase64String(vRandomBytes), DateTime.Now.AddMinutes(_vJwtOptions.Value.RefreshTokenExpireMinutes));
    }
}

