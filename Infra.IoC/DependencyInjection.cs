using Application.Login.Interfaces;
using Application.Login.Services;
using Application.Usuario.Interfaces;
using Application.Usuario.Services;
using Domain.Entities;
using Domain.Usuario.Interfaces;
using Infra.Data.Context;
using Infra.Data.Repository.Login;
using Infra.Data.Repository.Usuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<ILogin, LoginRepository>();
            services.AddScoped<ILoginServices, LoginServices>();
            services.AddScoped<IUsuario, UsuariosRepository>();
            services.AddScoped<IUsuarioServices, UsuarioServices>();

            return services;
        }
    }
}
