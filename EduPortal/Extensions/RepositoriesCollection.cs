using EAU.Repository.Repositories;
using EduPortal.Application.Interfaces.Repositories;

namespace EAU.API.Extensions
{
    public static class RepositoriesCollection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISubscriberRepository, SubscriberRepository>();

            return services;
            
        }
    }
}
