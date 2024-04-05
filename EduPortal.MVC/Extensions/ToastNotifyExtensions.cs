using Microsoft.Extensions.DependencyInjection;
using NToastNotify;

namespace EduPortal.MVC.Extensions
{
    public static class ToastNotifyExtensions
    {
        public static void AddToastNotify(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .AddNToastNotifyToastr(new ToastrOptions
                    {
                        PositionClass = ToastPositions.TopRight,
                        TimeOut = 3000
                    });
        }
    }
}
