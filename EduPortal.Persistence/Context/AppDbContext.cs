
using EduPortal.Domain.Entities;
using EduPortal.Persistence.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EduPortal.API.Models;
using Microsoft.AspNetCore.Identity;

namespace EduPortal.Persistence.context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SaveChangesInterceptor(contextAccessor)).UseLazyLoadingProxies()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


    }

}
