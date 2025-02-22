
using CalculatorWebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using pizzeria_web_api.Models;
using pizzeria_web_api.Repositories;
using pizzeria_web_api.Services;
using System.Text;

namespace pizzeria_web_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>().Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                // questo evento permette di leggere il token dal cookie 
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddControllers();
            builder.Services.AddSingleton<ICustomLogger, CustomConsoleLogger>();
            builder.Services.AddSingleton<PizzaRepository>();
            builder.Services.AddSingleton<CategoriaRepository>();
            builder.Services.AddSingleton<IngredienteRepository>();
            builder.Services.AddScoped<IPasswordHasher<UtenteModel>, PasswordHasher<UtenteModel>>();
            builder.Services.AddScoped<JwtAuthenticationService>();
            builder.Services.AddScoped<UtenteService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<LogMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            
            app.Run();
        }
    }
}
