using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Messaging;
using EduPortal.Application.Services;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Repositories;
using EduPortal.Persistence.Services;
using EduPortal.Service.Services;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IQueueRepository, QueueRepository>();

            //services.AddScoped<RabbitMQPublisher>();
            //services.AddScoped<RabbitMQConsumer>();

            services.AddScoped<OutboxMessageProcessor>();
            //services.AddSingleton<IServiceScopeFactory, ServiceScopeFactory>(); // Add IServiceScopeFactory


            //services.AddScoped<IRabbitMQConsumerService>();

            services.AddSingleton<RabbitMQConsumerService>();
            services.AddScoped<IRabbitMQPublisherService, RabbitMQPublisherService>();



            services.AddScoped<SubscriberNotificationService>();

            services.AddScoped<ISubscriberTerminateService, SubscriberTerminateService>();

            //services.AddSingleton<RabbitMQConnectionManager>();

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAddressService, AddressService>();






            return services;
        }
    }
}
