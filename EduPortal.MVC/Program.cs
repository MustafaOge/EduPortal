using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Validations.Subscriber;
using EduPortal.Domain.Entities;
using EduPortal.Models.Entities;
using EduPortal.MVC.Extensions;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Mapping;
using EduPortal.Persistence.Repositories;
using EduPortal.Persistence.Services;
using EduPortal.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddToastNotify();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScopedWithExtension();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(
    //options => options.UseSqlServer(builder.Configuration.GetConnectionString("")
    );



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

var app = builder.Build();

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


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseNToastNotify();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
