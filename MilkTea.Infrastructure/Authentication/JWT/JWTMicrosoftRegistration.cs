using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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
                        // có gửi token nhưng validate thất bại (hết hạn / sai chữ ký / sai issuer/audience / token lỗi)
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.Headers.Append("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        // thường là không gửi token hoặc token không dùng được
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
                                timestamp = DateTime.UtcNow,
                                error = context.Error,
                                errorDescription = context.ErrorDescription
                            });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            return services;
        }
    }
}
