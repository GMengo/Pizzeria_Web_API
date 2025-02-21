
using CalculatorWebApi.Middlewares;
using pizzeria_web_api.Repositories;
using pizzeria_web_api.Services;

namespace pizzeria_web_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<ICustomLogger, CustomConsoleLogger>();
            builder.Services.AddSingleton<PizzaRepository>();
            builder.Services.AddSingleton<CategoriaRepository>();
            builder.Services.AddSingleton<IngredienteRepository>();
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

            app.UseMiddleware<LogMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            
            app.Run();
        }
    }
}
