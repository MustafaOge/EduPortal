using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Mapping;
using EduPortal.Persistence.Repositories;
using EduPortal.Service.Services;

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

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



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
