using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Data.DataSeeding;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using E_Commerce.Infrastructure.Identity.Services;
using E_Commerce.Infrastructure.Payments;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.SeedingData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure
{
    public static class InfrastructureServiceRegister
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeed>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPaymentGateway, StripePaymentGateway>();

            var jwtSetting = configuration.GetSection("JWT").Get<JWTSettings>()
                ?? throw new InvalidOperationException("JWT Settings is Empty !");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Request successded
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Request failed
            } ).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    // Validations
                    ValidateIssuer = true,
                    ValidIssuer = jwtSetting.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSetting.Audience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                    ClockSkew = TimeSpan.FromMinutes(5) // Allow 5 minutes till token expiration
                };
            });

            return services;
        }
    }
}
