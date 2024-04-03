using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Validations.Subscriber;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Mapping;
using EduPortal.Persistence.Repositories;
using EduPortal.Persistence.Services;
using EduPortal.Service.Services;
using FluentValidation.AspNetCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 3000

    });

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>();


builder.Services.AddScoped<ISubsIndividualRepository, SubsIndividualRepository>(); 
builder.Services.AddScoped<ISubsIndividualService, SubsIndividualService>();


builder.Services.AddScoped<ISubsCorporateRepository, SubsCorporateRepository>();
builder.Services.AddScoped<ISubsCorporateService, SubsCorporateService>();

builder.Services.AddScoped<IFakeDataService, CreateFakeDataService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();


builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<SubsCorporateRepository>();
builder.Services.AddScoped<SubsIndividualRepository>();

builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();



builder.Services
.AddControllers()
.AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<CreateSubsIndividualDtoValidator>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseNToastNotify();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
