using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extension
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServiceExtension(this IServiceCollection services,IConfiguration config)
        {
            services.AddScoped<ITokenService,TokenService>();
            services.AddDbContext<DataContext>(options=>{
                options.UseSqlite(config.GetConnectionString("DefalutConnection"));
            });

            return services;
        }
    }
}