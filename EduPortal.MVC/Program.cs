using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.HangfireJobs.Schedules;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Messaging;
using EduPortal.Application.Services;
using EduPortal.Application.Validations.Subscriber;
using EduPortal.Domain.Entities;
using EduPortal.Models.Entities;
using EduPortal.MVC.Controllers;
using EduPortal.MVC.Extensions;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Mapping;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;
using StackExchange.Redis;
using System.Globalization;
using System.Reflection;
using Serilog;
using Serilog.Sinks.Graylog;
using EduPortal.MVC.Middleware;
using RabbitMQ.Client;
using MassTransit;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);



#region Localizer

builder.Services.AddSingleton<LanguageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization(options =>
    options.DataAnnotationLocalizerProvider = (type, factory) =>
    {
        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
        return factory.Create(nameof(SharedResource), assemblyName.Name);
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("fr-FR"),
        new CultureInfo("tr-TR"),
    };
    options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
    options.SupportedCultures = supportCultures;
    options.SupportedUICultures = supportCultures;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});
#endregion

//builder.Services.AddHostedService<DbInitializerHostedService>();

// Add services to the container.
builder.Services.AddToastNotify();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScopedWithExtension();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(
    //options => options.UseSqlServer(builder.Configuration.GetConnectionString("")
    );
//builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:1500");

var hangfireDbConnectionStrings = builder.Configuration.GetConnectionString("HangfireConnection");

builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(hangfireDbConnectionStrings);
});

builder.Services.AddHangfireServer();


builder.Services
.AddControllers()
.AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<CreateSubsIndividualDtoValidator>());



builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequiredLength = 3;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.Password.RequireDigit = false;


}).AddEntityFrameworkStores<AppDbContext>();



builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    opt.SlidingExpiration = true; //
    opt.Cookie.Name = "EduPortal";
    opt.Cookie.SameSite = SameSiteMode.Strict; // çapraz siteleri kabul etme
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; //
    opt.LoginPath = new PathString("/Home/SignIn");

});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<IMailService, MailService>();


builder.Services.AddSingleton(sp =>
{
    var connectionFactory = new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
    };

    var connection = connectionFactory.CreateConnection();

    var channel = connection.CreateModel();
    channel.ConfirmSelect();

    channel.ExchangeDeclare("payment2.main.queueu", ExchangeType.Fanout, true, false, null);
    return channel;
});

// MassTransit yapýlandýrmasý
builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<OutageNotificationConsumer>();
    x.AddConsumer<PaymentStartingMessageConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!), host => { });

        config.UseMessageRetry(r => r.Immediate(5));
        config.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));

        config.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15),
            TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45)));

        config.UseInMemoryOutbox(context);

        config.ReceiveEndpoint("outage-notification-queue", e => e.ConfigureConsumer<OutageNotificationConsumer>(context));
        config.ReceiveEndpoint("payment2.main.queueu", configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumer<PaymentStartingMessageConsumer>(context);
        });
    });
});

builder.Services.AddSingleton(sp =>
{
    var connectionFactory = new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
    };

    return connectionFactory.CreateConnection();
});






var app = builder.Build();

 Log.Logger = new LoggerConfiguration().WriteTo.Graylog(new GraylogSinkOptions()
{
    Facility = "GraylogClient",

    HostnameOrAddress = "localhost",
    Port = 12201,
    TransportType = Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp
}).CreateLogger();

var appLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();


appLifetime.ApplicationStarted.Register(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        await cacheService.CacheSubscribersAsync();
   

    }
});





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var fakeDataService = scope.ServiceProvider.GetRequiredService<IFakeDataService>();
    fakeDataService.CreateFakeData();
}


////app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseNToastNotify();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


if (!TestServerOptions.TestServer)
{

    app.UseHangfireDashboard("/hangfire", new DashboardOptions()
    {
        DashboardTitle = "Hangfire Dashboard",
        Authorization = new[]{
    new HangfireBasicAuthenticationFilter.HangfireCustomBasicAuthenticationFilter{
        User = builder.Configuration.GetSection("HangfireCredentials:UserName").Value,
        Pass = builder.Configuration.GetSection("HangfireCredentials:Password").Value
    }}
    });

    RecurringJobs.StartMessageService();


}




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }


//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<PaymentStartingMessageConsumer_copy>();
//    x.UsingRabbitMq((context, config) =>
//    {
//        config.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!), host => { });

//        config.UseMessageRetry(r => r.Immediate(5));
//        config.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));


//        config.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15),
//            TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45)));

//        config.UseInMemoryOutbox(context);




//        config.ReceiveEndpoint("payment2.order.created.event_copy",
//            configureEndpoint => { configureEndpoint.ConfigureConsumer<PaymentStartingMessageConsumer_copy>(context); });
//    });
//});

//builder.Services.AddSingleton(sp =>
//{
//    var connectionFactory = new ConnectionFactory()
//    {
//        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
//    };

//    return connectionFactory.CreateConnection();
//});


//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<PaymentStartingMessageConsumer>();

//    x.UsingRabbitMq((context, config) =>
//    {
//        config.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!), host => { });

//        config.UseMessageRetry(r => r.Immediate(5));
//        config.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));

//        config.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15),
//            TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45)));

//        config.UseInMemoryOutbox(context);



//        // Receive endpoint'lerini yapılandırma
//        //config.ReceiveEndpoint("stock.order.created.queue", configureEndpoint =>
//        //{
//        //    configureEndpoint.ConfigureConsumer<OrderCreatedEventConsumerWithMassTransit>(context);
//        //});

//        config.ReceiveEndpoint("payment2.order.created.event", configureEndpoint =>
//        {
//            configureEndpoint.ConfigureConsumer<PaymentStartingMessageConsumer>(context);
//        });
//    });
//});

// RabbitMQ bağlantısı için singleton servisi ekleme
//builder.Services.AddSingleton(sp =>
//{
//    var connectionFactory = new ConnectionFactory()
//    {
//        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
//    };

//    return connectionFactory.CreateConnection();
//});




//builder.Services.AddSingleton(sp =>
//{
//    var connectionFactory = new ConnectionFactory()
//    {
//        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
//    };

//    return connectionFactory.CreateConnection();
//});


//builder.Services.AddSingleton(sp =>
//{
//    var connectionFactory = new ConnectionFactory()
//    {
//        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
//    };

//    var connection = connectionFactory.CreateConnection();

//    var channel = connection.CreateModel();
//    channel.ConfirmSelect();

//    channel.ExchangeDeclare("payment2.order.created.event_copy", ExchangeType.Fanout, true, false, null);
//    return channel;
//});

//// MassTransit yapýlandýrmasý
//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<PaymentStartingMessageConsumer>();
//    x.AddConsumer<PaymentStartingMessageConsumer_copy>();


//    x.UsingRabbitMq((context, config) =>
//    {
//        config.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!), host => { });

//        config.UseMessageRetry(r => r.Immediate(5));
//        config.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));

//        config.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15),
//            TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45)));

//        config.UseInMemoryOutbox(context);

//        // Receive endpoint'lerini yapýlandýrma
//        //config.ReceiveEndpoint("stock.order.created.queue", configureEndpoint =>
//        //{
//        //    configureEndpoint.ConfigureConsumer<OrderCreatedEventConsumerWithMassTransit>(context);
//        //});

//        config.ReceiveEndpoint("payment2.main.queueu", configureEndpoint =>
//        {
//            configureEndpoint.ConfigureConsumer<PaymentStartingMessageConsumer>(context);
//        });


//        config.ReceiveEndpoint("payment2.order.created.event_copy" +
//            "",
//          configureEndpoint => { configureEndpoint.ConfigureConsumer<PaymentStartingMessageConsumer_copy>(context); });
//    });
//});

//// RabbitMQ baðlantýsý için singleton servisi ekleme
//builder.Services.AddSingleton(sp =>
//{
//    var connectionFactory = new ConnectionFactory()
//    {
//        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
//    };

//    return connectionFactory.CreateConnection();
//});
