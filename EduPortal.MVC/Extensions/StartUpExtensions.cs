using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Services;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Repositories;
using EduPortal.Persistence.Services;
using EduPortal.Service.Services;

namespace EduPortal.MVC.Extensions
{
    public static class StartUpExtensions
    {

        public static IServiceCollection AddScopedWithExtension(this IServiceCollection services)
        {
            services.AddScoped<ISubsIndividualRepository, SubsIndividualRepository>();
            services.AddScoped<ISubsIndividualService, SubsIndividualService>();


            services.AddScoped<ISubsCorporateRepository, SubsCorporateRepository>();
            services.AddScoped<ISubsCorporateService, SubsCorporateService>();

            services.AddScoped<IFakeDataService, CreateFakeDataService>();

            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceService, InvoiceService>();


            services.AddScoped<SubsCorporateRepository>();
            services.AddScoped<SubsIndividualRepository>();

            services.AddScoped<ISubscriberRepository, SubscriberRepository>();


            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICacheService, CacheService>();




            return services;
        }
    }
}
