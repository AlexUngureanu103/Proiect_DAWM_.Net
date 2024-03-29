﻿using Core.Services;
using DataLayer;
using DataLayer.Repositories;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Domain.ServicesAbstractions;

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
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IIngredientsService, IngredientsService>();
            services.AddScoped<IMenusService, MenusService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IDishesTypeService, DishesTypeService>();
            services.AddScoped<IOrdersService, OrdersService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IDishesTypeRepository, DishesTypeRepository>();
            services.AddScoped<IMenusRepository, MenusRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
