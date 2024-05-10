using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PhraseFluent.API.ExceptionHandling;
using PhraseFluent.DataAccess;
using PhraseFluent.DataAccess.Repositories;
using PhraseFluent.DataAccess.Repositories.Interfaces;
using PhraseFluent.Service;
using PhraseFluent.Service.AutoMapper;
using PhraseFluent.Service.Interfaces;
using PhraseFluent.Service.Options;

namespace PhraseFluent.API;

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
            opt.UseMySQL(configuration.GetValue<string>("DataBase:ConnectionString") ?? throw new InvalidOperationException(),
                b => {
                    b.MigrationsAssembly("PhraseFluent.Website");
                    b.CommandTimeout(60);
                });
        });

        #region scopes and configuration

        services.Configure<MicrosoftTranslatorSettings>(builder.Configuration.GetSection("Translator"));
        services.Configure<TokenConfiguration>(tokenOptionsSection);

        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITestsService, TestsService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBaseRepository, BaseRepository>();
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        
        services.AddAutoMapper(typeof(AppMappingProfile));
        services.AddSingleton(key);
        #endregion

        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseCors("AllowSpecificOrigin");

        app.UseRouting();
        
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