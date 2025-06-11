
using DemoWebAppFifthDay.Middlewares;

namespace DemoWebAppFifthDay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();


            app.MapControllers();

            app.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("NNoooooOOooo");
        }
    }
}
