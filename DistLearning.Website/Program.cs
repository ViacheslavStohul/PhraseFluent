using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DistLearning.API.ExceptionHandling;
using DistLearning.DataAccess;
using DistLearning.DataAccess.Repositories;
using DistLearning.DataAccess.Repositories.Interfaces;
using DistLearning.Service;
using DistLearning.Service.AutoMapper;
using DistLearning.Service.Interfaces;
using DistLearning.Service.Options;

namespace DistLearning.API;

using System.Threading.RateLimiting;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;
        var services = builder.Services;
        
        var tokenOptionsSection = configuration.GetSection("Authorization");
        var tokenConfiguration = new TokenConfiguration();
        tokenOptionsSection.Bind(tokenConfiguration);

        var key = GenerateSecurityKey();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                conf => conf.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        
        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Phrase fluent API documentation",
                Version = "v1",
                Description = "List of APIs"
            });
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetValue<string>("DataBase:ConnectionString") ?? throw new InvalidOperationException(),
                b => {
                    b.MigrationsAssembly("DistLearning.Website");
                    b.CommandTimeout(60);
                });
        });
        
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: "global",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0 
                    }));

            options.AddPolicy("TokenPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: "global",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromHours(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
            
            options.RejectionStatusCode = 429;
            
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(
                    new ErrorDetails
                    {
                        StatusCode = 429,
                        Message = "Занадто багато запитів. Спробуйте пізніше"
                    }.ToString(),
                    cancellationToken);
            };
        });

        #region scopes and configuration

        services.Configure<MicrosoftTranslatorSettings>(builder.Configuration.GetSection("Translator"));
        services.Configure<TokenConfiguration>(tokenOptionsSection);

        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITestsService, TestsService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBaseRepository, BaseRepository>();
        services.AddScoped<ITestRepository, TestRepository>();
        
        services.AddAutoMapper(typeof(AppMappingProfile));
        services.AddSingleton(key);
        #endregion

        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseCors("AllowSpecificOrigin");

        app.UseRouting();
        
        app.UseRateLimiter();
        
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => Results.Ok("Ok"));

        app.UseExceptionHandling();
        
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            await context?.Database.MigrateAsync()!;
            await context?.Initialize()!;
        }
        
        app.Run();
    }
    
    private static SymmetricSecurityKey GenerateSecurityKey()
    {
        var key = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return new SymmetricSecurityKey(key);
    }
}