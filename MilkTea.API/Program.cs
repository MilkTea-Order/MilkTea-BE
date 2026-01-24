using Microsoft.OpenApi.Models;
using MilkTea.API.RestfulAPI.Middlewares;
using MilkTea.Application.UseCases;
using MilkTea.Application.Ports.Identity;
using MilkTea.API.RestfulAPI.Common;
using MilkTea.Infrastructure.Authentication.JWT;
using MilkTea.Infrastructure.Database;
using MilkTea.Infrastructure.Database.MySQL;
using MilkTea.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add JWT authentication
builder.Services.AddAuthenticationJWTMicrosoft(builder.Configuration, builder.Environment);

// Current user abstraction (Application port)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

// Add connect database service
builder.Services.AddConnectDatabase(builder.Configuration, new MySQLProvider(builder.Configuration));

// Add repository services
builder.Services.AddRepositories();

// Add usecase services
builder.Services.AddUsecases();

// Add services to the container.
builder.Services.AddControllers();

// Disable automatic 400 response for invalid ModelState
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Swagger configuration
/*
 * Registers API Explorer support for Minimal APIs.
 *
 * Since ASP.NET Core uses Endpoint Routing, all routes are represented as endpoints.
 * Controller-based APIs create endpoints through the Application Model,
 * while Minimal APIs create endpoints directly.
 *
 * AddEndpointsApiExplorer registers an endpoint-based IApiDescriptionProvider
 * that reads endpoint metadata to create ApiDescription objects,
 * which Swagger uses to generate the OpenAPI document for Minimal APIs.
 */
builder.Services.AddEndpointsApiExplorer();

// Registers the Swagger generator
/*
 * AddSwaggerGen recieve configuration action
 * And using OpenApiInfo class adds information such as the author, license, and description.
 */
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("docs", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Milk Tea Order System",
        Description = "APIs for Milk Tea Order System"
    });

    // Include XML comments if present
    // Get the XML documentation file name generated during build
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // Get the full path to the XML documentation file
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Config JWT Authentication for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
        "Enter your JWT token in the text input below.\r\n\r\n" +
        "Example: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
        // Name of the header used to pass the token
        Name = "Authorization",
        // Location of the token (HTTP header)
        In = ParameterLocation.Header,
        // Defines the security scheme type (HTTP authentication)
        Type = SecuritySchemeType.Http,
        // Specifies the HTTP authentication scheme (Bearer)
        Scheme = "bearer",
        // Indicates the token format (JWT) for documentation purposes
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { new OpenApiSecurityScheme {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
            Array.Empty<string>()
        }
    });
});
#endregion


builder.Services.AddAutoMapper(typeof(Program).Assembly);


//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = context =>
//    {
//        var errors = context.ModelState
//            .Where(x => x.Value.Errors.Count > 0)
//            .ToDictionary(
//                x => x.Key,
//                x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
//            );

//        var result = new
//        {
//            success = false,
//            message = "fkejhfkrh",
//            errors = errors
//        };

//        return new BadRequestObjectResult(result);
//    };
//});


var app = builder.Build();

#region Swagger UI configuration
// Enable swagger only Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "{documentName}/swagger.json";
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("docs/swagger.json", "API v1.0");
        options.RoutePrefix = string.Empty;
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}
#endregion
app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
