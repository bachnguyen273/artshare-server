//using artshare_server.Infrastructure.AutoMapper;
using artshare_server.Services.ServiceExtension;
using artshare_server.WebAPI.WebAPIExtension;

namespace artshare_server.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDIServices(builder.Configuration);
            builder.Services.AddDIWebAPI(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}