using EduPortal.Application.Interfaces;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Persistence.Services;
using EduPortal.Repository.Repositories;


namespace EduPortal.API.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IConsumerRepository,ConsumerRepository>();
            return services;
        }
    }
}
