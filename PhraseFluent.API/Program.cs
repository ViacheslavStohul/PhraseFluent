using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PhraseFluent.DataAccess;
using PhraseFluent.Service;

namespace PhraseFluent.API;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var configuration = builder.Configuration;
        
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
        });

        builder.Services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetValue<string>("DataBase:ConnectionString"),
                b => {
                    b.MigrationsAssembly("PhraseFluent.API");
                    b.CommandTimeout(60);
                });
        });

        #region scopes

        builder.Services.AddScoped<IWordService, WordService>();
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
}