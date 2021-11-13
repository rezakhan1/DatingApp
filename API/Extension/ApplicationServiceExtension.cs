using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helper;
using API.Interface;
using API.Service;
using API.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extension
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServiceExtension(this IServiceCollection services,IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IPhotoService,PhotoService>();
            services.Configure<CloudinarySetting>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ILikesRepository,LikeRepository>();
            services.AddScoped<IMessageRepository,MessageRepository>();
            
            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);
            services.AddDbContext<DataContext>(options=>{
                options.UseSqlite(config.GetConnectionString("DefalutConnection"));
            });
            services.AddScoped<IUserRepository,UserRepository>();
            return services;
        }
    }
}