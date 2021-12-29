using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            //Token Generator Dependency Injection
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IphotoService,PhotoService>();
            services.AddScoped<LogUserActivity>();
            //User Repository
            services.AddScoped<IUserRepository, UserRepository>();
            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            //DB Context Dependency Injection
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
