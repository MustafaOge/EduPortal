using EAU.API.Extensions;
using EduPortal;
using EduPortal.API.Extensions;
using EduPortal.API.Models;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.ConfigureIdentity();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddScoped<UserManager<IdentityUser>>();
//builder.Services.AddScoped<RoleManager<IdentityRole>>();
//builder.Services.AddScoped<SignInManager<IdentityUser>>();


//builder.Services.AddDbContext<AppDbContext>(
//    options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("mssqlconnection")); }) ;

builder.Services.AddRepositories().AddServices();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddIdentityCore<IdentityUser>()
//                                 .AddEntityFrameworkStores<AppDbContext>()
//                                 .AddApiEndpoints();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequiredLength = 4;
//    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@.";
//    options.User.RequireUniqueEmail = true;
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
//    options.Lockout.MaxFailedAccessAttempts = 5;
//}).AddEntityFrameworkStores<AppDbContext>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

//app.UseEndpoints(endpoints =>
//{
//    //endpoints.MapDefaultControllerRoute("")
//}
//);
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.ConfigureDefaultAdminUser();

app.Run();
