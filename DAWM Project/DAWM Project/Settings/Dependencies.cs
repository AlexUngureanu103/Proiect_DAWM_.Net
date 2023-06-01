using Core.Services;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;

namespace DAWM_Project.Settings
{
    public static class Dependencies
    {
        public static void Inject(WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddControllers();
            applicationBuilder.Services.AddSwaggerGen();

            applicationBuilder.Services.AddDbContext<AppDbContext>();

            AddRepositories(applicationBuilder.Services);
            AddServices(applicationBuilder.Services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<UsersService>();
            services.AddScoped<AuthorizationService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<UsersRepository>();
            services.AddScoped<UnitOfWork>();
        }
    }
}
