﻿//using Microsoft.AspNetCore.Identity;

//namespace EduPortal
//{
//    public static class ApplicationExtension
//    {
//        public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
//        {
//            const string adminUser = "Admin";
//            const string adminPassword = "Admin+123456";

//            // UserManager
//            UserManager<IdentityUser> userManager = app
//                .ApplicationServices
//                .CreateScope()
//                .ServiceProvider
//                .GetRequiredService<UserManager<IdentityUser>>();

//            // RoleManager
//            RoleManager<IdentityRole> roleManager = app
//                .ApplicationServices
//                .CreateAsyncScope()
//                .ServiceProvider
//                .GetRequiredService<RoleManager<IdentityRole>>();

//            IdentityUser user = await userManager.FindByNameAsync(adminUser);
//            if (user is null)
//            {
//                user = new IdentityUser()
//                {
//                    Email = "zcomert@samsun.edu.tr",
//                    PhoneNumber = "5061112233",
//                    UserName = adminUser,
//                };

//                var result = await userManager.CreateAsync(user, adminPassword);

//                if (!result.Succeeded)
//                    throw new Exception("Admin user could not been created.");

//                var roleResult = await userManager.AddToRolesAsync(user,
//                    roleManager
//                        .Roles
//                        .Select(r => r.Name)
//                        .ToList()
//                );

//                if (!roleResult.Succeeded)
//                    throw new Exception("System have problems with role defination for admin.");
//            }
//        }
//    }
//}
