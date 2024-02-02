using artshare_server.Core.Interfaces;
using artshare_server.Infrastructure.AutoMapper;
using artshare_server.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace artshare_server.Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BachDatabase"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IArtworkRepository, ArtworkRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IWatermarkRepository, WatermarkRepository>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}