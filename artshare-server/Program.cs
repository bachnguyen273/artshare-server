using artshare_server.Infrastructure.ServiceExtension;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;

namespace artshare_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDIServices(builder.Configuration);
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IArtworkService, ArtworkService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<ILikeService, LikeService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IWatermarkService, WatermarkService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}