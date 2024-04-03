
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
using Microsoft.Extensions.Options;
using EduPortal.Domain.Entities;

namespace EduPortal.Persistence.context
{
    public class AppDbContext(IHttpContextAccessor contextAccessor) : DbContext
    {
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SubsIndividual> Individuals { get; set; }
        public DbSet<SubsCorporate> Corprorates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<InvoiceComplaint> invoiceComplaints { get; set; }






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=EduPortal; Trusted_Connection=true;TrustServerCertificate=True;").AddInterceptors(new SaveChangesInterceptor(contextAccessor)).UseLazyLoadingProxies()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                optionsBuilder.UseLazyLoadingProxies(false);
            }
            base.OnConfiguring(optionsBuilder);

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured == false)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost;Database=EduPortal; Trusted_Connection=true;TrustServerCertificate=True;");
        //        optionsBuilder.UseLazyLoadingProxies(false);
        //    }
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Subscriber>().HasDiscriminator().HasValue<SubsIndividual>("SubsIndividuals").HasValue<SubsCorporate>("SubsCorporates");

            //modelBuilder.Entity<SubsIndividual>().HasDiscriminator<string>("Discriminator").HasValue<SubsIndividual>("SubsIndividual");
            //modelBuilder.Entity<SubsCorporate>().HasDiscriminator<string>("Discriminator").HasValue<SubsCorporate>("SubsCorporate");


            modelBuilder.Entity<SubsIndividual>().ToTable("SubsIndividuals");
            modelBuilder.Entity<SubsCorporate>().ToTable("SubsCorporates");
        }


    }

}