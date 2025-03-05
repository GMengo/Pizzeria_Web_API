
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
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseAuthentication();
            app.UseMiddleware<LogMiddleware>();
            app.UseAuthorization();

            // mettendo il middleware del logger dopo l' autenticazione e l' autorizzazione Le risposte di "401 Unauthorized" e "403 Forbidden" generate da UseAuthentication()/UseAuthorization() non raggiungeranno il logging middleware, quindi non verranno registrate. 
            // se invece lo mettessi prima non avrei nella request le credenziali dell' utente che sta effettuando la chiamata API
            // soluzione migliore per il momento è metterlo tra Authentication e Autorization, in modo tale che autentica sempre l' utente prima dell' utilizzo del logger in modo tale da poter vedere quando vengono autorizzati e quando no

            app.UseHttpsRedirection();
            app.MapControllers();

            
            app.Run();
        }
    }
}
