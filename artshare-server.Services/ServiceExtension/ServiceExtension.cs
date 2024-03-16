using artshare_server.Core.Interfaces;
using artshare_server.Infrastructure.AutoMapper;
using artshare_server.Infrastructure.Repositories;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace artshare_server.Services.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IArtworkService, ArtworkService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IWatermarkService, WatermarkService>();
            services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();

            services.AddScoped(x => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}