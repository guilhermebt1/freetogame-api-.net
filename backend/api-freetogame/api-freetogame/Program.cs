using api_freetogame.Interfaces;
using api_freetogame.Services;
using Scalar.AspNetCore;

namespace api_freetogame;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient("FreeToGame", client =>
        {
            string? baseUrl = builder.Configuration.GetValue<string>("FreeToGameApi:BaseUrl");
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("A configuração 'FreeToGameApi:BaseUrl' não foi encontrada ou está vazia.");
            }
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }); 
            
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<IFreeToGameService, FreeToGameService>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                policy =>
                {
                    policy.WithOrigins(
                            "http://127.0.0.1:5173",
                            "http://localhost:5173",
                            "https://guilhermebt1.github.io")
                        .WithMethods("GET")
                        .AllowAnyHeader();
                }
            );
        });
        
        
        var app = builder.Build();

       
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}