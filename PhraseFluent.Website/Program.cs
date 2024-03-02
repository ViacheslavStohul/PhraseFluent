using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PhraseFluent.DataAccess;
using PhraseFluent.Service;
using PhraseFluent.Service.Options;

namespace PhraseFluent.API;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var tokenOptionsSection = builder.Configuration.GetSection("Authorization");
        var tokenConfiguration = new TokenConfiguration();
        tokenOptionsSection.Bind(tokenConfiguration);

        var key = GenerateSecurityKey();
        
        var configuration = builder.Configuration;

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        
        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o =>
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

        builder.Services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetValue<string>("DataBase:ConnectionString"),
                b => {
                    b.MigrationsAssembly("PhraseFluent.Website");
                    b.CommandTimeout(60);
                });
        });

        #region scopes and configuration

        builder.Services.Configure<MicrosoftTranslatorSettings>(builder.Configuration.GetSection("Translator"));
        builder.Services.Configure<TokenConfiguration>(tokenOptionsSection);

        builder.Services.AddScoped<ITranslationService, TranslationService>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddSingleton(key);
        #endregion

        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.MapControllers();

        app.MapGet("/", () => Results.Ok("Ok")); //Health check
        
        
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<DataContext>())
            {
                context?.Database.Migrate();
            }
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