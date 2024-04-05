
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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using EduPortal.Domain.Entities;
using EduPortal.Models.Entities;
using EduPortal.Models.Configurations;
using EduPortal.Persistence.Configurations;

namespace EduPortal.Persistence.context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>

    {
        private readonly IHttpContextAccessor _contextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor)
           : base(options)
        {
            _contextAccessor = contextAccessor;
        }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SubsIndividual> Individuals { get; set; }
        public DbSet<SubsCorporate> Corprorates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<InvoiceComplaint> invoiceComplaints { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<AppUserProfile> Profiles { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=EduPortal; Trusted_Connection=true;TrustServerCertificate=True;").AddInterceptors(new SaveChangesInterceptor(_contextAccessor)).UseLazyLoadingProxies()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                optionsBuilder.UseLazyLoadingProxies(false);
            }
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());


            modelBuilder.Entity<SubsIndividual>().ToTable("SubsIndividuals");
            modelBuilder.Entity<SubsCorporate>().ToTable("SubsCorporates");
        }


    }

}