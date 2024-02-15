using artshare_server.Services.ServiceExtension;
using artshare_server.WebAPIExtension;

namespace artshare_server
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