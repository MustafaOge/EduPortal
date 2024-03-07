using EduPortal.Application.Interfaces;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Services;
using EduPortal.Repository.Repositories;
using Microsoft.AspNetCore.Identity;


namespace EduPortal.API.Extensions
{
    public static class ServiceCollection
    {
        //public static void ConfigureIdentity(this IServiceCollection services)
        //{
        //    services.AddIdentity<IdentityUser, IdentityRole>(options =>
        //    {
        //        options.SignIn.RequireConfirmedAccount = false;
        //        options.User.RequireUniqueEmail = true;
        //        options.Password.RequireUppercase = false;
        //        options.Password.RequireLowercase = false;
        //        options.Password.RequireDigit = false;
        //        options.Password.RequiredLength = 6;
        //    }).AddEntityFrameworkStores<AppDbContext>();
        //}


        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IConsumerRepository,ConsumerRepository>();
            return services;
        }
    }
}
